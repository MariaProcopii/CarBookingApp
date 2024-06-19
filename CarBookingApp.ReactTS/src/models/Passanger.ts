import { TVehicleDetail } from "./VehicleDetail";

export type TPassanger = {
    id: number,
    firstName: string,
    lastName: string,
    gender: string,
    dateOfBirth: string, 
    email: string, 
    phoneNumber: string,
    yearsOfExperience: number | null,
    vehicleDetail: TVehicleDetail | null, 
};