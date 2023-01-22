function changeQueuePrivacy() {
    var checkbox = document.getElementsByClassName('queue-privacy')[0];
    var passwordInput = document.getElementById('password-value');
    passwordInput.disabled = !(checkbox === null || checkbox === void 0 ? void 0 : checkbox.checked);
}
//# sourceMappingURL=create-queue.js.map