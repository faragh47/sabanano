//var pinContainer = document.getElementsByClassName("pin-code")[0];
var pinContainer = document.querySelector(".pin-code");
console.log('There is ' + pinContainer.length + ' Pin Container on the page.');

pinContainer.addEventListener('keyup', function (event) {
    var target = event.srcElement;

    var maxLength = parseInt(target.attributes["maxlength"].value, 10);
    var myLength = target.value.length;

    if (myLength >= maxLength) {
        var next = target;
        while (next = next.nextElementSibling) {
            if (next == null) break;
            if (next.tagName.toLowerCase() == "input") {
                next.focus();
                break;
            }
        }
    }

    if (myLength === 0) {
        var next = target;
        while (next = next.previousElementSibling) {
            if (next == null) break;
            if (next.tagName.toLowerCase() == "input") {
                next.focus();
                break;
            }
        }
    }
}, false);

pinContainer.addEventListener('keydown', function (event) {
    var target = event.srcElement;
    target.value = "";
}, false);