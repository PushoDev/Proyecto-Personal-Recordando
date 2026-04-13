import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box, AppBar, Toolbar, InputBase, IconButton, Avatar, Menu, MenuItem, Typography, Divider
} from '@mui/material';
import { Search, Notifications } from '@mui/icons-material';
import { useAuth } from '../../context/AuthContext';

const drawerWidth = 240;
const collapsedWidth = 72;

interface TopBarProps {
  sidebarOpen: boolean;
}

export default function TopBar({ sidebarOpen }: TopBarProps) {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <AppBar
      position="fixed"
      sx={{
        width: `calc(100% - ${sidebarOpen ? drawerWidth : collapsedWidth}px)`,
        ml: `${sidebarOpen ? drawerWidth : collapsedWidth}px`,
        backgroundColor: '#fff',
        color: '#333',
        boxShadow: '0 1px 3px rgba(0,0,0,0.1)',
        transition: (theme) => theme.transitions.create(['width', 'margin'], {
          easing: theme.transitions.easing.sharp,
          duration: theme.transitions.duration.enteringScreen,
        }),
      }}
    >
      <Toolbar>
        <Box sx={{ display: 'flex', alignItems: 'center', backgroundColor: '#f5f5f5', borderRadius: 1, px: 2, py: 0.5, flex: 1, maxWidth: 400 }}>
          <Search sx={{ color: '#666', mr: 1 }} />
          <InputBase
            placeholder="Buscar..."
            sx={{ flex: 1, fontSize: 14 }}
          />
        </Box>

        <Box sx={{ flexGrow: 1 }} />

        <IconButton sx={{ mr: 1 }}>
          <Notifications />
        </IconButton>

        <IconButton onClick={handleMenu} sx={{ p: 0 }}>
          <Avatar sx={{ bgcolor: '#1a1a2e', width: 36, height: 36 }}>
            {user?.nombreCompleto?.charAt(0) || 'U'}
          </Avatar>
        </IconButton>

        <Menu
          anchorEl={anchorEl}
          open={Boolean(anchorEl)}
          onClose={handleClose}
          transformOrigin={{ horizontal: 'right', vertical: 'top' }}
          anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
        >
          <MenuItem disabled>
            <Typography variant="body2">{user?.nombreCompleto}</Typography>
          </MenuItem>
          <MenuItem disabled>
            <Typography variant="caption" color="text.secondary">{user?.email}</Typography>
          </MenuItem>
          <Divider />
          <MenuItem onClick={handleLogout}>Cerrar sesión</MenuItem>
        </Menu>
      </Toolbar>
    </AppBar>
  );
}