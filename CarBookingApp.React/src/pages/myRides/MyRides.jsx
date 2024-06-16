import { Grid, Box, Container, Grow }  from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useAuth } from '../../components/provider/AuthProvider';
import { useTokenDecoder } from '../../utils/TokenUtils';
import Ride from '../../components/ride/Ride';
import axios from 'axios';
import Pagination from '@mui/material/Pagination';

export default function MyRides() {

    const [rides, setRides] = useState([]);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const { token } = useAuth();
    const claims = useTokenDecoder(token);

    const fetchRides = () => {
    
        axios.get(`http://192.168.0.9:5239/ride/created/${claims.nameidentifier}?PageNumber=${pageIndex}`)
          .then((response) => {
              setRides(response.data.items);
              setTotalPages(response.data.totalPages);
              console.log(response.data.items);
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
            <Box mb={5} />
            <Grid container spacing={5} direction='row' wrap='wrap' alignItems='center' justifyContent='center' flexGrow={2}>
                {rides.map((ride) => (
                    <Grid item xs={6} sm={5} md={4} lg={3} key={ride.id}>
                        <Grow in={true} timeout={500}>
                            <div>
                                <Ride ride={ride} edit={true} />
                            </div>
                        </Grow>
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
