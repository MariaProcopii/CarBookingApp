import { Grid, Box }  from '@mui/material';
import Ride from '../../components/ride/Ride';
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { parseErrorMessages } from '../../utils/ErrorUtils';
import SearchBar from '../../components/searchBar/SearchBar';

export default function AvailableRides() {

    const [backendErrors, setBackendErrors] = useState({});
    const [rides, setRides] = useState([]);

    const fetchRides = () => {
        axios.get("http://localhost:5239/ride/21")
        .then((response) => {
            setRides(response.data.items);
            console.log(response.data.items);
        })
        .catch((error) => {
            const { data } = error.response;
            console.log('here2');
            setBackendErrors(parseErrorMessages(data.Message));
          });
    };


  // useEffect(() => {
  //   fetchRides();
  // }, []);
  useEffect(() => {
    setTimeout(() => {
      fetchRides();
    }, 1000);
  }, []);

  return (
    <>
        <Grid container direction='row' alignItems='center' justifyContent='center'>
            <Grid item>
                <SearchBar />
            </Grid>
        </Grid>
        <Box mb={5} />
        <Grid container spacing={5} direction='row' wrap='wrap' alignItems='center' justifyContent='center'>
        {rides.map((ride) => (
          <Grid item xs={6} sm={5} md={4} lg={3} key={ride.id} >
            <Ride ride={ride}/>
          </Grid>
        ))}
        </Grid>
    </>
  )
}