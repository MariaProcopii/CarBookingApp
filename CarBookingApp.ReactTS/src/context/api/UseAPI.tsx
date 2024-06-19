import APIContext from "./APIContext";
import { useContext } from "react";

export default function useAPI() {
    return useContext(APIContext);
}