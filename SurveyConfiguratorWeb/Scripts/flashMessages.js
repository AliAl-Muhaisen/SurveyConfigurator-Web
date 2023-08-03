function SuccessFlash(message) {
     window.FlashMessage.success(message, FlashObject());
}
function ErrorFlash(message) {
    window.FlashMessage.error(message, FlashObject());
}


function FlashObject() {
    return {
        progress: true,
        interactive: true,
        timeout: 8000,
        appear_delay: 200,
        container: '.flash-container',
        theme: 'default',
        classes: {
            container: 'flash-container',
            flash: 'flash-message',
            visible: 'is-visible',
            progress: 'flash-progress',
            progress_hidden: 'is-hidden'
        }
    };
}