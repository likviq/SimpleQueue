function changeQueuePrivacy() {
    const checkbox = document.getElementsByClassName(
        'queue-privacy',
    )[0] as HTMLInputElement | null;

    const passwordInput = document.getElementById(
        'password-value',
    ) as HTMLInputElement | null;

    passwordInput.disabled = !checkbox?.checked
}

function addTag() {
    let elem = document.querySelector('.tag-body:last-of-type');

    let clone = elem.cloneNode(true);

    elem.getElementsByClassName('tag-input')[0]
        .textContent = "empty";

    elem.after(clone);
}

function deleteTag(element) {
    element.parentNode.remove();
}
