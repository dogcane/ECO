ko.bindingHandlers.date = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        if (value != null) {
            var jsonDate = new Date(parseInt(valueAccessor().substr(6)));
            element.innerHTML = jQuery.datepicker.formatDate(jsDateFormat, jsonDate);
        }
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
    }
};

ko.bindingHandlers.spinner = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = allBindingsAccessor().spinnerOptions || {};
        $(element).spinner(options);
        ko.utils.registerEventHandler(element, "spinchange", function () {
            var observable = valueAccessor();
            observable($(element).spinner("value"));
        });
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).spinner("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        current = $(element).spinner("value");
        if (value !== current) {
            $(element).spinner("value", value);
        }
    }
};

// Here's a custom Knockout binding that makes elements shown/hidden via jQuery's fadeIn()/fadeOut() methods
// Could be stored in a separate utility library
ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
    },
    update: function (element, valueAccessor) {
        // Whenever the value subsequently changes, slowly fade the element in or out
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).fadeIn() : $(element).fadeOut();
    }
};

ko.bindingHandlers.error = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value != null) {
            var isInError = value.IsInError;
            var message = value.Message;
            if (message) {
                $(element).attr('title', message);
            } else {
                $(element).attr('title', '');
            }
            if (isInError) {
                $(element).addClass('error');
            } else {
                $(element).removeClass('error');
            }
        }
    }
};

function ErrorMessageViewModel(isInError, message) {
    var self = this;
    self.IsInError = isInError;
    self.Message = message;
}