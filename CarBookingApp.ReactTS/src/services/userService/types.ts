export interface LoginParams {
    email: string;
    password: string;
}

export interface RegisterParams {
    firstName: string;
    lastName: string;
    email: string;
    gender: string;
    password: string;
    confirm_password: string;
    phoneNumber: string;
    dateOfBirth: string;
}

export interface DetailsParams {
    firstName: string;
    lastName: string; 
    gender: string; 
    phoneNumber: string;
    dateOfBirth: string;
    email: string; 
    password: string;
    yearsOfExperience: number; 
}

export interface YearsOfExperienceParams {
    yearsOfExperience: number;
}