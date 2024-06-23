import { AxiosInstance } from "axios";
import { TRide } from "../../models/Ride";
import { TRideDetails } from "../../models/RideDetails";
import { TRideData } from "../../models/RideData";
import { TDestination } from "../../models/Destination";
import { 
    SearchParams, 
    VehicleDetailsParams,
} from "./types";

function RideService(instance: AxiosInstance | null) {

    async function fetchRides(
        nameIdentifier: string,
        pageIndex: number,
        setRides: (newRides: TRide[]) => void,
        setTotalPages: (newTotalPages: number) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get(`/ride/${nameIdentifier}?PageNumber=${pageIndex}`);
            setRides(response?.data.items);
            setTotalPages(response?.data.totalPages);
        } catch (error) {
            console.log(error);
        }
    } 

    async function fetchRidesWithParams(
        nameIdentifier: string,
        searchParams: SearchParams,
        setRides: (newRides: TRide[]) => void,
        setTotalPages: (newTotalPages: number) => void,
        handleClose: () => void,
    ): Promise<void> {
        try {
            handleClose();
            const query = `destinationFrom=${searchParams.destinationFrom}&destinationTo=${searchParams.destinationTo}&dateOfTheRide=${searchParams.date}`;
            const response = await instance?.get(`/ride/${nameIdentifier}?${query}`);
            setRides(response?.data.items);
            setTotalPages(response?.data.totalPages);
        } catch (error) {
            console.log(error);
        }
    }

    async function fetchDestinations(
        setDestinations: (newDestinations: Array<TDestination>) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get("/destination/pick/name");
            const topCities = response?.data.map((city: string) => ({ label: city }))
            setDestinations(topCities);
        } catch (error) {
            console.log(error);
        }
    }

    async function fetchRideDetails(
        rideId: number,
        setRideDetails: (newRideDetails: TRideDetails | null) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get(`/ride/details/${rideId}`);
            setRideDetails(response?.data);
        } catch (error) {
            console.log(error);
        }
    }

    async function fetchVendors(
        setVendors: (newVendors: []) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get("/vehicle/pick/vendor");
            setVendors(response?.data);
        } catch(error) {
            console.log(error);
        }
    }

    async function fetchModels(
        vendorName: string,
        setModels: (newModels: []) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get(`/vehicle/pick/model?vendor=${vendorName}`);
            setModels(response?.data);
        } catch(error) {
            console.log(error);
        }
    }

    async function fetchFacilities(
        setFacilities: (newFacilities: []) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get("/facility/pick/type");
            setFacilities(response?.data);
        } catch(error) {
            console.log(error);
        }
    }

    async function updateVehicleDetails(
        nameIdentifier: string,
        vehicleDetails: VehicleDetailsParams,
        setOpen: (state: boolean) => void,
        setVehicleDetail: (obj: any) => void, 
    ): Promise<void> {
        try {
            const response = await instance?.put(`/vehicledetail/info/update/${nameIdentifier}`, vehicleDetails);
            setOpen(false);
            setVehicleDetail(response?.data);
        } catch(error) {
            setOpen(true);
            console.log(error);
        }
    }

    async function createVehicleDetails(
        nameIdentifier: string,
        vehicleDetails: VehicleDetailsParams,
        setOpen: (state: boolean) => void,
        setVehicleDetail: (obj: any) => void,
    ): Promise<void> {
        try {
            const response = await instance?.post(`/vehicledetail/create/${nameIdentifier}`, vehicleDetails);
            setOpen(false);
            setVehicleDetail(response?.data);
            console.log("called vehicle detail");
        } catch(error) {
            setOpen(true);
            console.log(error);
        }
    }

    async function createRide(
        nameIdentifier: string,
        rideData: TRideData,
        setSnackbar: (obj: {open: boolean; message: string; severity: string;}) => void, 
    ): Promise<void> {
        try {
            const _ = await instance?.post(`/ride/create/${nameIdentifier}`, rideData);
            setSnackbar({ open: true, message: "Ride created successfully!", severity: "success" });
        } catch(error) {
            setSnackbar({ open: true, message: "Failed to create ride.", severity: "error" });
            console.log(error);
        }
    }
    
    async function editRide(
        rideID: number,
        infoParams: any,
        setRideDetails: (obj: any) => void,
        setSnackbar: (obj: {open: boolean; message: string; severity: string;}) => void,
    ): Promise<void> {
        try {
            const response = await instance?.put(`/ride/info/update/${rideID}`, infoParams);
            setRideDetails(response?.data);
            setSnackbar({ open: true, message: "Ride changed successfully!", severity: "success" });
        } catch(error) {
            setSnackbar({ open: true, message: "Failed to change ride!", severity: "error" });
            console.log(error);
        }
    }

    async function bookRide(
        infoParams: any,
        setSnackbar: (obj: {open: boolean; message: string; severity: string;}) => void,
    ): Promise<void> {
        try {
            const _ = await instance?.post("/ride/info/book", infoParams);
            setSnackbar({ open: true, message: "Ride booked successfully!", severity: "success" });
        } catch(error) {
            setSnackbar({ open: true, message: "Failed to book ride!", severity: "error" });
            console.log(error);
        }
    }

    async function unsubscribeFromRide(
        infoParams: any,
        setSnackbar: (obj: {open: boolean; message: string; severity: string;}) => void,
    ): Promise<void> {
        try {
            const _ = await instance?.put("/ride/info/unsubscribe", infoParams);
            setSnackbar({ open: true, message: "Unsubscribed from ride successfully!", severity: "success" });
        } catch(error) {
            setSnackbar({ open: true, message: "Failed to unsubscribe from ride!", severity: "error" });
            console.log(error);
        }
    }

    async function fetchBookedRides(
        nameIdentifier: string,
        pageIndex: number,
        setRides: (newRides: TRide[]) => void,
        setTotalPages: (newTotalPages: number) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get(`/ride/booked/${nameIdentifier}?PageNumber=${pageIndex}`);
            setRides(response?.data.items);
            setTotalPages(response?.data.totalPages);
        } catch(error) {
            console.log(error);
        }
    }
    
    async function deleteRide(
        rideID: number,
        setSnackbar: (obj: {open: boolean; message: string; severity: string;}) => void,
    ): Promise<void> {
        try {
            const _ = await instance?.delete(`/ride/delete/${rideID}`);
            setSnackbar({ open: true, message: "Delete ride successfully!", severity: "success" });
        } catch(error) {
            setSnackbar({ open: true, message: "Failed to delete from ride!", severity: "error" });
            console.log(error);
        }
    }

    async function fetchPendingRides(
        nameIdentifier: string,
        pageIndex: number,
        setRides: (newRides: TRide[]) => void,
        setTotalPages: (newTotalPages: number) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get(`/ride/pending/${nameIdentifier}?PageNumber=${pageIndex}`);
            setRides(response?.data.items);
            setTotalPages(response?.data.totalPages);
        } catch(error) {
            console.log(error);
        }
    }

    async function fetchCreatedRides(
        nameIdentifier: string,
        pageIndex: number,
        setRides: (newRides: TRide[]) => void,
        setTotalPages: (newTotalPages: number) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get(`/ride/created/${nameIdentifier}?PageNumber=${pageIndex}`);
            setRides(response?.data.items);
            setTotalPages(response?.data.totalPages);
        } catch(error) {
            console.log(error);
        }
    }

    async function fetchUserPendingRides(
        nameIdentifier: string,
        pageIndex: number,
        setPendingUsersInfo: (obj: any) => void,
        setTotalPages: (newTotalPages: number) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get(`/user/pending/${nameIdentifier}?PageNumber=${pageIndex}`);
            setPendingUsersInfo(response?.data.items);
            setTotalPages(response?.data.totalPages);
        } catch(error) {
            console.log(error);
        }
    }

    return {
        fetchRides,
        fetchRidesWithParams,
        fetchDestinations,
        fetchRideDetails,
        fetchVendors,
        fetchModels,
        fetchFacilities,
        updateVehicleDetails,
        createVehicleDetails,
        createRide,
        editRide,
        bookRide,
        unsubscribeFromRide,
        fetchBookedRides,
        deleteRide,
        fetchPendingRides,
        fetchCreatedRides,
        fetchUserPendingRides,
    };
}

export default RideService;