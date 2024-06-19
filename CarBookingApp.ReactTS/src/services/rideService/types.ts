export interface SearchParams {
    date: string;
    destinationFrom: string;
    destinationTo: string;
    seats: number;
}

export interface VehicleDetailsParams {
    manufactureYear: number;
    registrationNumber: string;
    vehicle: {
        vender: string;
        model: string;
    };
}