import { TOwner } from "./Owner";
import { TPassanger } from "./Passanger";

export type TRideDetails = {
    id: number,
    dateOfTheRide: string, 
    destinationFrom: string,
    destinationTo: string,
    totalSeats: number,
    owner: TOwner,
    rideDetail: {
        pickUpSpot: string, 
        price: number,
        facilities: string[],
    },
    passengers: TPassanger[],
};