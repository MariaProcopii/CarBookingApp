export function parseErrorMessages(errorMessage) {
    const errorObject = {};
    const messages = errorMessage.split('.').filter(Boolean);

    messages.forEach(message => {
        const [key, value] = message.split(':').map(str => str.trim());
        if (errorObject[key]) {
            errorObject[key] += ` ${value}`;
        } else {
            errorObject[key] = value;
        }
    });

    return errorObject;
}