function SpeakerListItem(code, name, surname) {
    var self = this;
    self.code = ko.observable(code);
    self.name = ko.observable(name);
    self.surname = ko.observable(surname);
}

function SpeakerEditViewModel(code, name, surname, description, age) {
    var self = this;
    self.code = ko.observable(code);
    self.name = ko.observable(name);
    self.surname = ko.observable(surname);
    self.description = ko.observable(description);
    self.age = ko.observable(age);

    self.currentListItem = null;
    self.setCurrentListItem = function (data) {
        self.currentListItem = data;
    };
    self.refreshCurrentListItem = function () {
        if (self.currentListItem != null) {
            self.currentListItem.name(self.name());
            self.currentListItem.surname(self.surname());
        }
    };

    self.isNewSpeaker = ko.computed(function () {
        return self.code() == '';
    });

    self.toListItem = function () {
        return new SpeakerListItem(self.code(), self.name(), self.surname());
    };
}

function SpeakersListViewModel() {
    var self = this;
    self.speakers = ko.observableArray([]);
    self.currentSpeaker = ko.observable(null);

    self.Load = function () {
        $.ajax({
            url: '/speakers/api/?nameOrSurname=',
            type: 'GET',
            dataType: 'json'
        })
        .done(function (data) {
            $.each(data, function (index, speaker) {
                self.speakers.push(new SpeakerListItem(speaker.SpeakerCode, speaker.Name, speaker.Surname));
            });
        })
    };

    self.Save = function () {
        if (self.currentSpeaker().isNewSpeaker()) {
            self.__Create();
        } else {
            self.__Update();
        }
    };

    self.Edit = function (data) {
        self.currentSpeaker(new SpeakerEditViewModel('', '', '', '', ''));
    };

    self.Abort = function () {
        self.currentSpeaker(null);
    };

    self.Delete = function (data) {
        self.currentSpeaker(null);
        $.ajax({
            url: '/speakers/api/' + data.code(),
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
            alert(jqXHR.statusText);
        });
    };

    self.Update = function (data) {
        $.ajax({
            url: '/speakers/api/' + data.code(),
            type: 'GET',
            dataType: 'json'
        })
       .done(function (result) {
           if (result.Success) {
               self.currentSpeaker(new SpeakerEditViewModel(result.Value.SpeakerCode, result.Value.Name, result.Value.Surname, result.Value.Description, result.Value.Age));
               self.currentSpeaker().setCurrentListItem(data);
           } else {
               alert('Error');
           }
       })
    }

    self.__Create = function () {
        $.ajax({
            url: '/speakers/api',
            type: 'POST',
            dataType: 'json',
            data: {
                Name: self.currentSpeaker().name(),
                Surname: self.currentSpeaker().surname(),
                Description: self.currentSpeaker().description(),
                Age: self.currentSpeaker().age()
            }
        })
        .done(function (result) {
            if (result.Success) {
                self.currentSpeaker().code(result.Value);
                self.speakers.push(self.currentSpeaker().toListItem());
                self.currentSpeaker(null);
            } else {
                alert('Error');
            }
        })
        .fail(function (jqXHR, textStatus) {
            alert(textStatus);
        })
    };

    self.__Update = function () {
        $.ajax({
            url: '/speakers/api',
            type: 'PUT',
            dataType: 'json',
            data: {
                SpeakerCode : self.currentSpeaker().code(),
                Name: self.currentSpeaker().name(),
                Surname: self.currentSpeaker().surname(),
                Description: self.currentSpeaker().description(),
                Age: self.currentSpeaker().age()
            }
        })
        .done(function (result) {
            if (result.Success) {
                self.currentSpeaker().refreshCurrentListItem();
                self.currentSpeaker(null);
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
    var vm = new SpeakersListViewModel();
    vm.Load();
    ko.applyBindings(vm);
});