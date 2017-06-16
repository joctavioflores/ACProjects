var moneyObservable = (function ($) {
    var cleanInput = function (value) {
        return parseFloat(value.replace(/[^0-9.-]/, ''));
    };

    var format = function (value) {
        toks = value.toFixed(2).replace('-', '').split('.');
        var display = '$' + $.map(toks[0].split('').reverse(), function (elm, i) {
            return [(i % 3 == 0 && i > 0 ? ',' : ''), elm];
        }).reverse().join('') + '.' + toks[1];

        return value < 0 ? '(' + display + ')' : display;
    };

    return function (initialValue) {
        var raw = typeof initialValue == "function" ?
            ko.dependentObservable(initialValue) : ko.observable(initialValue);

        var public = ko.dependentObservable({
            read: function () { return raw() },
            write: function (value) { raw(cleanInput(value)) }
        });

        public.formatted = ko.dependentObservable({
            read: function () { return format(raw()) },
            write: function (value) { raw(cleanInput(value)) }
        });
        return public;
    };
})(jQuery);


//Not part of the money observable, just wiring the viewModel up to the bindings.
$(function () {
    var viewModel = {
        Cash: moneyObservable(-1234.56),
        Check: moneyObservable(2000),
        showJSON: function () {
            alert(ko.toJSON(viewModel));
        }
    };

    viewModel.Total = moneyObservable(function () {
        return viewModel.Cash() + viewModel.Check()
    });
    ko.applyBindings(viewModel);
});