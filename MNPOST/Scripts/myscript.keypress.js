var app = angular.module('myKeyPress', []);
app.directive('myEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.myEnter);
                });

                event.preventDefault();
            }
        });
    };
});

app.directive('myToolTip', function () {

    return {
        terminal: true,
        priority: 1001,
        link: function (scope, element, attributes) {
           // el.removeAttr('my-dir');
            element.attr('data-toggle', 'tooltip');
            element.attr('title', attributes.myToolTip);
        }
    };

});