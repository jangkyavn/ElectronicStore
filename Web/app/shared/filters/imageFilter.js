(function (app) {
    app.filter('imageFilter', function () {
        return function (input) {
            if (input == null || input.length == 0)
                return 'no-image.png';
            else
                return input;
        }
    });
})(angular.module('common.module'));