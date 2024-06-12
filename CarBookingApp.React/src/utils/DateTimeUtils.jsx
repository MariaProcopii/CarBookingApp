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

  export function transformDate(date) {
    const formatedDate = new Date(date).toLocaleString().split(",")[0];
    const [month, day, year] = formatedDate.split('/');

    if (month && day && year) {
        return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
    } else {
        return null;
    }
  };