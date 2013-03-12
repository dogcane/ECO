function EventListItemViewModel(code, name, startDate, endDate, numberOfSession) {
    var self = this;
    self.code = ko.observable(code);
    self.name = ko.observable(name);
    self.startDate = ko.observable(startDate);
    self.endDate = ko.observable(endDate);
    self.numberOfSession = ko.observable(numberOfSession);
}

function EventEditViewModel(code, name, description, startDate, endDate) {
    var self = this;
    self.code = ko.observable(code);
    self.name = ko.observable(name);
    self.description = ko.observable(description);
    self.startDate = ko.observable(startDate);
    self.endDate = ko.observable(endDate);
    self.sessions = ko.observableArray([]);

    self.isNewEvent = ko.computed(function () {
        return self.code() == '';
    });
}

function EventsListViewModel() {
    var self = this;
    self.events = ko.observableArray([]);
    self.currentEvent = ko.observable(null);

    self.LoadData = function () {
        $.ajax({
            url: '/events/api/?start=&end=&page=1&pageSize=100',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                $.each(data.CurrentElements, function (index, event) {
                    self.events.push(new EventListItemViewModel(event.EventCode, event.Name, event.StartDate, event.EndDate, event.NumberOfSessions));
                });
            }
        })
    };

    self.SaveEvent = function () {
        $.ajax({
            url: self.currentEvent().isNewEvent() ? '/events/api' : '/events/api/' + self.currentEvent().code(),
            type: self.currentEvent().isNewEvent() ? 'POST' : 'PUT',
            dataType: 'json',
            data: {
                name: self.currentEvent().name(),
                description: self.currentEvent().description(),
                startDate: self.currentEvent().startDate(),
                endDate: self.currentEvent().endDate()
            },
            success: function (data) {
                self.events.push(self.currentEvent());
                self.currentEvent(null);
            }
        })
    };

    self.StartNew = function (data) {
        var d = new Date();
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var todayDate = (day < 10 ? '0' : '') + day +'/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
        self.currentEvent(new EventEditViewModel('', '', '', todayDate, todayDate));
    };

    self.AbortEdit = function () {
        self.currentEvent(null);
    };
}

$(document).ready(function () {
    var vm = new EventsListViewModel();
    vm.LoadData();
    ko.applyBindings(vm);
});