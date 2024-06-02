import { jwtDecode } from "jwt-decode";

export function tokenDecoder(token) {
    if (!token) {
        return {};
    }
    const decodedToken = jwtDecode(token);
    const originalKeys = Object.keys(decodedToken);
    const newKeys = Object.keys(decodedToken).map(claim => {
            if (claim.startsWith("http")) {
                return claim.split('/').pop();
            } else {
                return claim;
            }
        }
    );
    const claimsObject = {};
    originalKeys.forEach((originalKey, index) => {
        const newKey = newKeys[index];
        claimsObject[newKey] = decodedToken[originalKey];
    });

    return claimsObject;
}