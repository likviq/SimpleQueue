function changeQueuePrivacy() {
    var checkbox = document.getElementsByClassName('queue-privacy')[0];
    var passwordInput = document.getElementById('password-value');
    passwordInput.disabled = !(checkbox === null || checkbox === void 0 ? void 0 : checkbox.checked);
}
function changeQueueType() {
    var checkboxDelayedQueue = document.getElementsByClassName('delayed-queue')[0];
    var ActiveTimeFromInput = document.getElementById('from-time');
    var ActiveTimeToInput = document.getElementById('to-time');
    var DurationTimeInput = document.getElementById('duration-time');
    ActiveTimeFromInput.disabled = !(checkboxDelayedQueue === null || checkboxDelayedQueue === void 0 ? void 0 : checkboxDelayedQueue.checked);
    ActiveTimeToInput.disabled = !(checkboxDelayedQueue === null || checkboxDelayedQueue === void 0 ? void 0 : checkboxDelayedQueue.checked);
    DurationTimeInput.disabled = !(checkboxDelayedQueue === null || checkboxDelayedQueue === void 0 ? void 0 : checkboxDelayedQueue.checked);
}
function addTag() {
    var elem = document.querySelector('.tag-body:last-of-type');
    var clone = elem.cloneNode(true);
    elem.getElementsByClassName('tag-input')[0]
        .textContent = "empty";
    elem.after(clone);
}
function deleteTag(element) {
    element.parentNode.remove();
}
//# sourceMappingURL=create-queue.js.map