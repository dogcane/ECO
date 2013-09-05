function SpeakerListItemViewModel(code, name, surname) {
    var self = this;
    self.code = ko.observable(code);
    self.name = ko.observable(name);
    self.surname = ko.observable(surname);
}

function SpeakerEditViewModel(code, name, surname, description, age) {
    var self = this;
    //CODE
    self.code = ko.observable(code);
    //NAME
    self.name = ko.observable(name);
    self.nameError = ko.observable(new ErrorMessageViewModel(false, ''));
    //SURNAME
    self.surname = ko.observable(surname);
    self.surnameError = ko.observable(new ErrorMessageViewModel(false, ''));
    //DESCRIPTION
    self.description = ko.observable(description);
    self.descriptionError = ko.observable(new ErrorMessageViewModel(false, ''));
    //AGE
    self.age = ko.observable(age);
    self.ageError = ko.observable(new ErrorMessageViewModel(false, ''));

    var errorMapping = {
        'Name': self.nameError,
        'Surname': self.surnameError,
        'Description': self.descriptionError,
        'Age': self.ageError
    };

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
        return new SpeakerListItemViewModel(self.code(), self.name(), self.surname());
    };

    self.resetErrors = function () {
        $.each(errorMapping, function () {
            this(new ErrorMessageViewModel(false, ''));
        });
    };

    self.setError = function (context, message) {
        errorMapping[context](new ErrorMessageViewModel(true, speakerResources.getResource(message)));
    };
}

function SpeakersListViewModel() {
    var self = this;
    self.speakers = ko.observableArray([]);
    self.currentSpeaker = ko.observable(null);
    self.NameOrSurnameFilter = ko.observable("");

    self.Load = function () {
        $.ajax({
            url: '/speakers/api?nameOrSurname=' + self.NameOrSurnameFilter(),
            type: 'GET',
            dataType: 'json'
        })
        .done(function (data) {
            self.speakers.removeAll();
            $.each(data, function (index, speaker) {
                self.speakers.push(new SpeakerListItemViewModel(speaker.SpeakerCode, speaker.Name, speaker.Surname));
            });
        })
        .fail(function (jqXHR, textStatus) {
            alert(textStatus);
        })
    };

    self.Save = function () {
        if (self.currentSpeaker().isNewSpeaker()) {
            __Create();
        } else {
            __Update();
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
            alert(textStatus);
        })
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
        .fail(function (jqXHR, textStatus) {
            alert(textStatus);
        })
    }

    function __Create() {
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
                self.currentSpeaker().resetErrors();
                $.each(result.Errors, function () {
                    self.currentSpeaker().setError(this.Context, this.Description);
                });
            }
        })
        .fail(function (jqXHR, textStatus) {
            alert(textStatus);
        })
    };

    function __Update() {        
        $.ajax({
            url: '/speakers/api',
            type: 'PUT',
            dataType: 'json',
            data: {
                SpeakerCode: self.currentSpeaker().code(),
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
                self.currentSpeaker().resetErrors();
                $.each(result.Errors, function () {
                    self.currentSpeaker().setError(this.Context, this.Description);
                });
            }
        })
        .fail(function (jqXHR, textStatus) {
            alert(textStatus);
            alert(jqXHR.status);
        })
    };
}

$(document).ready(function () {
    var vm = new SpeakersListViewModel();
    vm.Load();
    ko.applyBindings(vm);
});