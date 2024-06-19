export type TRideData = {
    dateOfTheRide: string;
    destinationFrom: string;
    destinationTo: string;
    totalSeats: number;
    rideDetail: {
        pickUpSpot: string;
        price: number;
        facilities: Array<any>;
    };
}; 
