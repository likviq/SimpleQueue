function changeQueuePrivacy() {
    const checkbox = document.getElementsByClassName(
        'queue-privacy',
    )[0] as HTMLInputElement | null;

    const passwordInput = document.getElementById(
        'password-value',
    ) as HTMLInputElement | null;

    passwordInput.disabled = !checkbox?.checked
}
