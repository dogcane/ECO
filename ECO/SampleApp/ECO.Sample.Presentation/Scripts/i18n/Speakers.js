function SpeakerResources () {
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
        'SURNAME_REQUIRED': {
            'global': 'The surname is required',
            'it-IT': 'Il cognome è obbligatorio'
        },
        'SURNAME_TOO_LONG': {
            'global': 'The surname is too long [MAX 50 CHARS]',
            'it-IT': 'Il cognome è troppo lungo [MAX 50 CAR]'
        },
        'DESCRIPTION_TOO_LONG': {
            'global': 'The description is too long [MAX 1000 CHARS]',
            'it-IT': 'La descrizione è troppo lunga [MAX 1000 CAR]'
        },
        'AGE_MUST_BE_OVER18': {
            'global': 'The age must be greater than 18',
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