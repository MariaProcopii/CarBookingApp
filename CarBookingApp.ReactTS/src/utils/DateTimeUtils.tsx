import { parseISO, format } from "date-fns";
import { differenceInYears } from "date-fns";

export function getDateFromISO(isoString: string) {
    if (!isoString) {
        return;
    }
    const date = parseISO(isoString);
    return format(date, "yyyy-MM-dd");
}

export function getTimeFromISO(isoString: string) {
    const date = parseISO(isoString);
    return format(date, "HH:mm");
}

export function calculateAge(dateOfBirth: string | number | Date) {
    return differenceInYears(new Date(), new Date(dateOfBirth));
}

export function transformDateTime(date: string) {
    const transformedDate = new Date(date).toISOString().split("Z")[0];

    if (transformedDate) {
        return transformedDate;
    } else {
        return null;
    }
}
