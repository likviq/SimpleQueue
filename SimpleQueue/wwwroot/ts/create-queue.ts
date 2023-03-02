function changeQueuePrivacy() {
    const checkbox = document.getElementsByClassName(
        'queue-privacy',
    )[0] as HTMLInputElement | null;

    const passwordInput = document.getElementById(
        'password-value',
    ) as HTMLInputElement | null;

    passwordInput.disabled = !checkbox?.checked
}

function changeQueueType() {
    const checkboxDelayedQueue = document.getElementsByClassName(
        'delayed-queue',
    )[0] as HTMLInputElement | null;

    const ActiveTimeFromInput = document.getElementById(
        'from-time',
    ) as HTMLInputElement | null;

    const ActiveTimeToInput = document.getElementById(
        'to-time',
    ) as HTMLInputElement | null;

    const DurationTimeInput = document.getElementById(
        'duration-time',
    ) as HTMLInputElement | null;

    ActiveTimeFromInput.disabled = !checkboxDelayedQueue?.checked
    ActiveTimeToInput.disabled = !checkboxDelayedQueue?.checked
    DurationTimeInput.disabled = !checkboxDelayedQueue?.checked
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

function setBgImage(input, target) {
    const image = document.getElementById(
        'image-holder',
    ) as HTMLInputElement | null;

    var fReader = new FileReader();

    fReader.readAsDataURL(image.files[0]);
    fReader.onloadend = function (event) {
        const img = document.getElementById(
            'image-place',
        ) as HTMLImageElement | null;
        img.src = event.target.result as string;
    }
}