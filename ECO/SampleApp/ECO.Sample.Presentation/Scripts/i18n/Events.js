function EventResources () {
    var self = this;
    var language = 'global';
    var resources = {        
        'NAME_REQUIRED': {
            'global': 'The name is required',
            'it-IT': 'Il nome è obbligatorio'
        },
        'NAME_TOO_LONG': {
            'global': 'The name is too long [MAX 50 CHARS]',
            'it-IT': 'Il nome è troppo lungo [MAX 50 CAR]'
        },
        'DESCRIPTION_REQUIRED': {
            'global': 'The description is required',
            'it-IT': 'La descrizione è obbligatoria'
        },
        'DESCRIPTION_TOO_LONG': {
            'global': 'The description is too long [MAX 200 CHARS]',
            'it-IT': 'La descrizione è troppo lunga [MAX 200 CAR]'
        },
        'STARTDATE_GREATER_ENDDATE': {
            'global': 'The start date must be greater than end date',
            'it-IT': 'L\'età deve essere maggiore di 18'
        }
    };

    self.setLanguage = function (lang) {
        language = lang;
    };

    self.getResource = function (resx) {
        var resource = resources[resx][language];
        if (!resource) resource = resources[resx]['global'];
        return resource;        
    };
}

var speakerResources = new SpeakerResources();