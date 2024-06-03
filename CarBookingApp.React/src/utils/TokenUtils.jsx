import { jwtDecode } from "jwt-decode";
import { useEffect, useState } from "react";

function tokenDecoder(token) {
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

export function useTokenDecoder(key = 'token') {
    const [claims, setClaims] = useState(tokenDecoder(localStorage.getItem(key)));

    useEffect(() => {
        const currentToken = localStorage.getItem(key);
        setClaims(tokenDecoder(currentToken));
    }, [key]);

    return claims;
}