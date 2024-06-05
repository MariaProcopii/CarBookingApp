import { parseISO, format } from 'date-fns';
import { differenceInYears } from 'date-fns';

  export function getDateFromISO(isoString) {
    if(!isoString){
      return;
    }
    const date = parseISO(isoString);
    return format(date, 'yyyy-MM-dd');
  }
  
  export function getTimeFromISO(isoString) {
    const date = parseISO(isoString);
    return format(date, 'HH:mm');
  }

  export function calculateAge(dateOfBirth) {
    return differenceInYears(new Date(), new Date(dateOfBirth));
};