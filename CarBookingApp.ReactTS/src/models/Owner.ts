import { TVehicleDetail } from "./VehicleDetail";

export type TOwner = {
    id: number,
    firstName: string,
    lastName: string,
    gender: string,
    dateOfBirth: string, 
    email: string, 
    phoneNumber: string, 
    yearsOfExperience: number,
    vehicleDetail: TVehicleDetail, 
};