import { JwtPayload, jwtDecode } from "jwt-decode";
import { useEffect, useState } from "react";

type ClaimObject = {
    [key: string]: string | string[] | undefined | number;
};

function tokenDecoder(token: string) {
    if (!token) {
        return {};
    }
    const decodedToken = jwtDecode(token);
    const originalKeys = Object.keys(decodedToken);
    const newKeys = Object.keys(decodedToken).map(claim => {
        if (claim.startsWith("http")) {
            return claim.split("/").pop();
        } else {
            return claim;
        }
    });

    const claimsObject = {} as ClaimObject;
    originalKeys.forEach((originalKey, index) => {
        const newKey = newKeys[index];
        claimsObject[newKey as keyof ClaimObject] = decodedToken[originalKey as keyof JwtPayload];
    });

    return claimsObject;
}

export function useTokenDecoder(token: string) {
    const [claims, setClaims] = useState(tokenDecoder(token));

    useEffect(() => {
        setClaims(tokenDecoder(token));
    }, [token]);

    return claims;
}

export function hasRole(claims: ClaimObject, requiredRole: string) {
    if (Array.isArray(claims.role)) {
        console.log("User has ");
        console.log(claims.role.includes(requiredRole));
        return claims.role.includes(requiredRole);
    }
    return false;
}
