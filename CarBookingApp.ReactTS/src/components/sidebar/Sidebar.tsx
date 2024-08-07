import * as React from "react";
import Box from "@mui/material/Box";
import MuiDrawer from "@mui/material/Drawer";
import MuiAppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import List from "@mui/material/List";
import CssBaseline from "@mui/material/CssBaseline";
import Typography from "@mui/material/Typography";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import MenuIcon from "@mui/icons-material/Menu";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import HomeIcon from "@mui/icons-material/Home";
import PlaceIcon from "@mui/icons-material/Place";
import EventAvailableIcon from "@mui/icons-material/EventAvailable";
import AccountCircleRoundedIcon from "@mui/icons-material/AccountCircleRounded";
import TimeToLeaveRoundedIcon from "@mui/icons-material/TimeToLeaveRounded";
import ExitToAppRoundedIcon from "@mui/icons-material/ExitToAppRounded";
import Brightness6Icon from "@mui/icons-material/Brightness6";
import HourglassBottomIcon from "@mui/icons-material/HourglassBottom";
import GroupAddIcon from "@mui/icons-material/GroupAdd";
import useAuth from "../../context/auth/UseAuth";
import useTheme from "../../context/theme/UseTheme";
import { Outlet } from "react-router-dom";
import { useTokenDecoder, hasRole } from "../../utils/TokenUtils";
import { AppBarProps as MuiAppBarProps } from "@mui/material/AppBar";
import { styled, Theme, CSSObject } from "@mui/material/styles";

const drawerWidth = 240;

const openedMixin = (theme: Theme): CSSObject => ({
    width: drawerWidth,
    transition: theme.transitions.create("width", {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.enteringScreen,
    }),
    overflowX: "hidden",
});

const closedMixin = (theme: Theme): CSSObject => ({
    transition: theme.transitions.create("width", {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
    }),
    overflowX: "hidden",
    width: `calc(${theme.spacing(7)} + 1px)`,
    [theme.breakpoints.up("sm")]: {
        width: `calc(${theme.spacing(8)} + 1px)`,
    },
});

const DrawerHeader = styled("div")(({ theme }) => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "flex-end",
    padding: theme.spacing(0, 1),
    ...theme.mixins.toolbar,
}));

interface AppBarProps extends MuiAppBarProps {
    open?: boolean;
}

const AppBar = styled(MuiAppBar, {
    shouldForwardProp: (prop) => prop !== "open",
})<AppBarProps>(({ theme, open }) => ({
    zIndex: theme.zIndex.drawer + 1,
    transition: theme.transitions.create(["width", "margin"], {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
    }),
    ...(open && {
        marginLeft: drawerWidth,
        width: `calc(100% - ${drawerWidth}px)`,
        transition: theme.transitions.create(["width", "margin"], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.enteringScreen,
        }),
    }),
}));

const Drawer = styled(MuiDrawer, { shouldForwardProp: (prop) => prop !== "open" })(
    ({ theme, open }) => ({
        width: drawerWidth,
        flexShrink: 0,
        whiteSpace: "nowrap",
        boxSizing: "border-box",
        ...(open && {
            ...openedMixin(theme),
            "& .MuiDrawer-paper": openedMixin(theme),
        }),
        ...(!open && {
            ...closedMixin(theme),
            "& .MuiDrawer-paper": closedMixin(theme),
        }),
    }),
);

export default function Sidebar() {
    const [open, setOpen] = React.useState(false);

    const { theme, isDarkThemeOn, setDarkTheme } = useTheme();
    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const handleDrawerOpen = () => {
        setOpen(true);
    };

    const handleDrawerClose = () => {
        setOpen(false);
    };

    const pickMenuIcon = (name: string) => {
        switch (name) {
            case "Home":
                return <HomeIcon />;
            case "Booked Rides":
                return <PlaceIcon />;
            case "Pending Rides":
                return <HourglassBottomIcon />;
            case "Pending Passengers":
                return <GroupAddIcon />;
            case "Create ride":
                return <EventAvailableIcon />;
            case "Profile":
                return <AccountCircleRoundedIcon />;
            case "My Rides":
                return <TimeToLeaveRoundedIcon />;
            case "Log out":
                return <ExitToAppRoundedIcon />;
            default:
                return <></>;
        }
    };

    const pickMenuRoute = (name: string) => {
        switch (name) {
            case "Home":
                return "/";
            case "Booked Rides":
                return "/booked-rides";
            case "Pending Rides":
                return "/pending-rides";
            case "Pending Passengers":
                return "/pending-passengers";
            case "Create ride":
                return "/create-ride";
            case "Profile":
                return "/profile";
            case "My Rides":
                return "/my-rides";
            case "Log out":
                return "/logout";
            default:
                return "/";
        }
    };

    const menuItems = [
        "Home",
        "Booked Rides",
        "Pending Rides",
        "Pending Passengers",
        "Create ride",
        "Profile",
        "My Rides",
        "Log out"
    ];

    const driverMenuItems = [
        "Create ride",
        "My Rides",
        "Pending Passengers",
    ];

    return (
        <>
            <Box sx={{ ml: 10, mb: 5, mr: 2 }}>
                <Box sx={{ display: "flex" }}>
                    <CssBaseline />
                    <AppBar position="fixed" open={open}>
                        <Toolbar>
                            <IconButton
                                color="inherit"
                                aria-label="open drawer"
                                onClick={handleDrawerOpen}
                                edge="start"
                                sx={{
                                    marginRight: 5,
                                    ...(open && { display: "none" }),
                                }}
                            >
                                <MenuIcon />
                            </IconButton>
                            <Typography variant="h6" noWrap component="div">
                                {claims?.surname + " " + claims?.name}
                            </Typography>
                            <IconButton
                                sx={{
                                    position: "absolute",
                                    marginLeft: "86vw"
                                }}
                                onClick={() => { setDarkTheme(!isDarkThemeOn) }}
                            >
                                {
                                    isDarkThemeOn
                                        ? <Brightness6Icon sx={{ color: "white" }} />
                                        : <Brightness6Icon sx={{ color: "black" }} />
                                }
                            </IconButton>
                        </Toolbar>
                    </AppBar>
                    <Drawer variant="permanent" open={open}>
                        <DrawerHeader>
                            <IconButton onClick={handleDrawerClose}>
                                {theme?.direction === "rtl" ? <ChevronRightIcon /> : <ChevronLeftIcon />}
                            </IconButton>
                        </DrawerHeader>
                        <Divider />
                        <List>
                            {menuItems.map((text) => {
                                if (driverMenuItems.includes(text) && !hasRole(claims, "Driver")) {
                                    return <></>;
                                }
                                return (
                                    <ListItem key={text} disablePadding sx={{ display: "block", }}>
                                        <ListItemButton
                                            sx={{
                                                minHeight: 48,
                                                justifyContent: open ? "initial" : "center",
                                                px: 2.5,
                                            }}
                                            component="a"
                                            href={pickMenuRoute(text)}
                                        >
                                            <ListItemIcon
                                                sx={{
                                                    minWidth: 0,
                                                    mr: open ? 2 : "auto",
                                                    justifyContent: "center",
                                                }}
                                            >
                                                {pickMenuIcon(text)}
                                            </ListItemIcon>
                                            <ListItemText primary={text} sx={{ opacity: open ? 1 : 0 }} />
                                        </ListItemButton>
                                    </ListItem>
                                )
                            })}
                        </List>
                        <Divider />
                    </Drawer>
                    <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
                        <DrawerHeader />
                    </Box>
                </Box>
                <Outlet />
            </Box>
        </>
    );
}
