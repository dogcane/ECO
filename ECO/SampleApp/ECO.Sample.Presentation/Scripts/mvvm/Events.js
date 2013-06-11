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
            url: '/events/api/?start=&end=&eventName=',
            type: 'GET',
            dataType: 'json'
        })
        .done(function (data) {
            $.each(data, function (index, event) {
                self.events.push(new EventListItemViewModel(event.EventCode, event.Name, event.StartDate, event.EndDate, event.NumberOfSessions));
            });
        })
    };

    self.Save = function () {
        if (self.currentEvent().isNewSpeaker()) {
            __Create();
        } else {
            __Update();
        }
    };    

    self.Edit = function (data) {
        self.currentEvent(new EventEditViewModel('', '', '', new Date(), new Date()));
    };

    self.Abort = function () {
        self.currentEvent(null);
    };

    self.Delete = function (data) {
        self.currentEvent(null);
        $.ajax({
            url: '/events/api/' + data.code(),
            type: 'DELETE',
            dataType: 'json'
        })
        .done(function (result) {
            if (result.Success) {
                self.speakers.remove(data);
            } else {
                alert('Error');
            }
        })
        .fail(function (jqXHR, textStatus) {
            alert(textStatus);
        })
    };

    self.Update = function (data) {
        $.ajax({
            url: '/events/api/' + data.code(),
            type: 'GET',
            dataType: 'json'
        })
        .done(function (result) {
            if (result.Success) {
                self.currentEvent(new EventEditViewModel(result.Value.EventCode, result.Value.Name, result.Value.Description, result.Value.StartDate, result.Value.EndDate));
                self.currentEvent().setCurrentListItem(data);
            } else {
                alert('Error');
            }
        })
        .fail(function (jqXHR, textStatus) {
            alert(textStatus);
        })
    }
    function __Create() {
        $.ajax({
            url: '/events/api',
            type: 'POST',
            dataType: 'json',
            data: {
                Name: self.currentEvent().name(),
                Description: self.currentEvent().description(),
                StartDate: self.currentEvent().startDate(),
                EndDate: self.currentEvent().endDate()
            }
        })
        .done(function (result) {
            if (result.Success) {
                self.currentEvent().code(result.Value);
                self.speakers.push(self.currentEvent().toListItem());
                self.currentEvent(null);
            } else {
                self.currentEvent().resetErrors();
                $.each(result.Errors, function () {
                    self.currentEvent().setError(this.Context, this.Description);
                });
            }
        })
        .fail(function (jqXHR, textStatus) {
            alert(textStatus);
        })
    };

    function __Update() {
        $.ajax({
            url: '/events/api',
            type: 'PUT',
            dataType: 'json',
            data: {
                EventCode: self.currentEvent().code(),
                Name: self.currentEvent().name(),
                Description: self.currentEvent().description(),
                StartDate: self.currentEvent().startDate(),
                EndDate: self.currentEvent().endDate()
            }
        })
        .done(function (result) {
            if (result.Success) {
                self.currentEvent().refreshCurrentListItem();
                self.currentEvent(null);
            } else {
                alert('Error');
            }
        })
        .fail(function (jqXHR, textStatus) {
            alert(textStatus);
        })
    };

}

$(document).ready(function () {
    var vm = new EventsListViewModel();
    vm.LoadData();
    ko.applyBindings(vm);
});