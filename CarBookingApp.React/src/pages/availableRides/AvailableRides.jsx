import { Grid, Box, Container }  from '@mui/material';
import Ride from '../../components/ride/Ride';
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { parseErrorMessages } from '../../utils/ErrorUtils';
import SearchBar from '../../components/searchBar/SearchBar';
import Pagination from '@mui/material/Pagination';
import Stack from '@mui/material/Stack';

export default function AvailableRides() {

    const [backendErrors, setBackendErrors] = useState({});
    const [rides, setRides] = useState([]);
    const [pageIndex, setPageIndex] = React.useState(1);
    const [totalPages, setTotalPages] = React.useState(1);


    const fetchRidesWithParams = (searchParams) => {
      const query = `destinationFrom=${searchParams.destinationFrom}&destinationTo=${searchParams.destinationTo}&dateOfTheRide=${searchParams.date}`;
    
      axios.get(`http://localhost:5239/ride/21?${query}`)
        .then((response) => {
            setRides(response.data.items);
        })
        .catch((error) => {
            const { data } = error.response;
            setBackendErrors(parseErrorMessages(data.Message));
          });
    };

    const fetchRides = () => {
    
      axios.get(`http://localhost:5239/ride/21?PageNumber=${pageIndex}`)
        .then((response) => {
            setRides(response.data.items);
            setTotalPages(response.data.totalPages);
        })
        .catch((error) => {
            const { data } = error.response;
            setBackendErrors(parseErrorMessages(data.Message));
          });
    };

  useEffect(() => {
    setTimeout(() => {
      fetchRides();
    }, 10);
  }, [pageIndex]);

  return (
      <Container sx={{ display: 'flex', flexDirection: 'column', minHeight: '80vh', justifyContent: 'center'}}>
          <Grid container direction='row' alignItems='center' justifyContent='center'>
              <Grid item>
                  <SearchBar onSearch={fetchRidesWithParams} />
              </Grid>
          </Grid>
          <Box mb={5} />
          <Grid container spacing={5} direction='row' wrap='wrap' alignItems='center' justifyContent='center' flexGrow={2}>
              {rides.map((ride) => (
                  <Grid item xs={6} sm={5} md={4} lg={3} key={ride.id}>
                      <Ride ride={ride}/>
                  </Grid>
              ))}
          </Grid>
          <Box mb={10} />
          <Grid container direction='row' alignItems='center' justifyContent='center' >
              <Pagination count={totalPages} 
                          variant="outlined" 
                          color="primary" 
                          onChange={(e, value) => setPageIndex(value)}
                          sx={{
                            '& .MuiPaginationItem-root': {
                                fontSize: {
                                    xs: '0.75rem',
                                    sm: '0.875rem',
                                    md: '1rem',
                                    lg: '1.25rem'
                                },
                                padding: {
                                    xs: '4px', 
                                    sm: '6px',
                                    md: '8px',
                                    lg: '10px'
                                }
                            }
                        }}
              />
          </Grid>
      </Container>
  )
}