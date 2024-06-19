export type ErrorObject = {
    [key: string] : string;
};

export function parseErrorMessages(errorMessage : string) {
    const errorObject : ErrorObject = {};
    const messages = errorMessage.split(".").filter(Boolean);

    messages.forEach(message => {
        const [key, value] = message.split(":").map(str => str.trim());
        if (errorObject[key]) {
            errorObject[key] += ` ${value}`;
        } else {
            errorObject[key] = value;
        }
    });
    
    return errorObject;
}