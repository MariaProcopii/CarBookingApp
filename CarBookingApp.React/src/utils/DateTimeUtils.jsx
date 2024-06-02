import { parseISO, format } from 'date-fns';


export function getDateFromISO(isoString) {
    const date = parseISO(isoString);
    return format(date, 'yyyy-MM-dd');
  }
  
export function getTimeFromISO(isoString) {
    const date = parseISO(isoString);
    return format(date, 'HH:mm');
  }