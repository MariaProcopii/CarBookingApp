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

export function useTokenDecoder(token) {
    const [claims, setClaims] = useState(tokenDecoder(token));

    useEffect(() => {
        setClaims(tokenDecoder(token));
    }, [token]);

    return claims;
}

export function hasRole(claims, requiredRole) {
    if (Array.isArray(claims.role)) {
      return claims.role.includes(requiredRole);
    }
    return false;
  }