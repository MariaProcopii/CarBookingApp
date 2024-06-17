import { Grid, Box, Container, Dialog, DialogContent, useMediaQuery, Button, Grow }  from '@mui/material';
import Ride from '../../components/ride/Ride';
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { parseErrorMessages } from '../../utils/ErrorUtils';
import SearchBar from '../../components/searchBar/SearchBar';
import Pagination from '@mui/material/Pagination';
import { useTheme } from '@mui/material/styles';
import ManageSearchIcon from '@mui/icons-material/ManageSearch';
import { useAuth } from '../../components/provider/AuthProvider';
import { useTokenDecoder } from '../../utils/TokenUtils';

export default function AvailableRides() {

    const [backendErrors, setBackendErrors] = useState({});
    const [rides, setRides] = useState([]);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [open, setOpen] = useState(false);
    const { token } = useAuth();
    const claims = useTokenDecoder(token);

    const theme = useTheme();
    const isSmallScreen = useMediaQuery(theme.breakpoints.down('sm'));

    const buttonStyle={
        fontSize: {
            xs: '0.6rem',
            sm: '0.7rem',
            md: '0.8rem',
            lg: '0.9rem'
            }  
    };

    const fetchRidesWithParams = (searchParams) => {
      handleClose();
      const query = `destinationFrom=${searchParams.destinationFrom}&destinationTo=${searchParams.destinationTo}&dateOfTheRide=${searchParams.date}`;
      axios.get(`http://192.168.0.9:5239/ride/${claims.nameidentifier}?${query}`)
        .then((response) => {
            setRides(response.data.items);
            setTotalPages(response.data.totalPages);
        })
        .catch((error) => {
            const { data } = error.response;
            setBackendErrors(parseErrorMessages(data.Message));
          });
    };

    const fetchRides = () => {
    
      axios.get(`http://192.168.0.9:5239/ride/${claims.nameidentifier}?PageNumber=${pageIndex}`)
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

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
      <Container sx={{ display: 'flex', flexDirection: 'column', minHeight: '80vh', justifyContent: 'center'}}>
          <Grid container direction='row' alignItems='center' justifyContent='center'>
                {isSmallScreen ? (
                <Grid item>
                    <Button variant="contained" 
                            startIcon={<ManageSearchIcon fontSize='large' color='black' />}
                            sx={buttonStyle}
                            onClick={handleClickOpen}
                    >
                            Search
                        </Button>
                    <Dialog open={open} onClose={handleClose} fullWidth maxWidth="xs">
                        <DialogContent>
                            <SearchBar onSearch={fetchRidesWithParams} />
                        </DialogContent>
                    </Dialog>
                </Grid>
                ) : (
                <Grid item>
                    <SearchBar onSearch={fetchRidesWithParams} />
                </Grid>
                )}
          </Grid>
          <Box mb={5} />
          <Grid container spacing={5} direction='row' wrap='wrap' alignItems='center' justifyContent='center' flexGrow={2}>
              {rides.map((ride) => (
                  <Grid item xs={6} sm={5} md={4} lg={3} key={ride.id}>
                        <Grow in={true} timeout={500}>
                            <div>
                                <Ride ride={ride} action={"book"} />
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