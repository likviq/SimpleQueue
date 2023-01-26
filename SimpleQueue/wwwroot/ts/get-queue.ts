function deleteUserFromQueue(id: string) {
    /// ajax call delete user from queue

    const userField = document.getElementById(
        'user-' + id,
    ) as HTMLInputElement | null;

    userField.remove();
}