import { Grid }  from '@mui/material';
import Ride from '../../components/ride/Ride';
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { parseErrorMessages } from '../../utils/ErrorUtils';

export default function AvailableRides() {

    const [backendErrors, setBackendErrors] = useState({});
    const [rides, setRides] = useState([]);

    const fetchRides = () => {
        axios.get("http://localhost:5239/ride/21")
        .then((response) => {
            setRides(response.data.items);
            console.log(response.data.items)
        })
        .catch((error) => {
            const { data } = error.response;
            setBackendErrors(parseErrorMessages(data.Message));
          });
    };


  useEffect(() => {
    fetchRides();
  }, []);

  return (
    <>
        <Grid container spacing={5} direction='row' wrap='wrap'>
        {rides.map((ride) => (
          <Grid item xs={6} sm={5} md={4} lg={3} key={ride.id} >
            <Ride ride={ride}/>
          </Grid>
        ))}
        </Grid>
    </>
  )
}