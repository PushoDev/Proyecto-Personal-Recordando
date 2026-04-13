import { useNavigate, useLocation } from 'react-router-dom';
import {
  Box, Drawer, List, ListItem, ListItemButton, ListItemIcon, ListItemText,
  IconButton, Typography, Divider, useTheme
} from '@mui/material';
import {
  Dashboard, Inventory, Link as LinkIcon, Menu, ChevronLeft, Logout
} from '@mui/icons-material';

const drawerWidth = 240;
const collapsedWidth = 72;

interface SidebarProps {
  open: boolean;
  onToggle: () => void;
}

const menuItems = [
  { text: 'Dashboard', icon: <Dashboard />, path: '/dashboard' },
  { text: 'Inventario', icon: <Inventory />, path: '/inventario' },
  { text: 'URLs', icon: <LinkIcon />, path: '/urls' },
];

export default function Sidebar({ open, onToggle }: SidebarProps) {
  const navigate = useNavigate();
  const location = useLocation();
  const theme = useTheme();

  return (
    <Drawer
      variant="permanent"
      sx={{
        width: open ? drawerWidth : collapsedWidth,
        flexShrink: 0,
        '& .MuiDrawer-paper': {
          width: open ? drawerWidth : collapsedWidth,
          boxSizing: 'border-box',
          backgroundColor: '#1a1a2e',
          color: '#fff',
          transition: theme.transitions.create('width', {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.enteringScreen,
          }),
          overflowX: 'hidden',
        },
      }}
    >
      <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: open ? 'space-between' : 'center', p: 2 }}>
        {open && (
          <Typography variant="h6" sx={{ fontWeight: 700, color: '#fff' }}>
            Sistema
          </Typography>
        )}
        <IconButton onClick={onToggle} sx={{ color: '#fff' }}>
          {open ? <ChevronLeft /> : <Menu />}
        </IconButton>
      </Box>

      <Divider sx={{ borderColor: 'rgba(255,255,255,0.1)' }} />

      <List>
        {menuItems.map((item) => (
          <ListItem key={item.text} disablePadding>
            <ListItemButton
              onClick={() => navigate(item.path)}
              selected={location.pathname === item.path}
              sx={{
                minHeight: 48,
                justifyContent: open ? 'initial' : 'center',
                px: 2.5,
                '&.Mui-selected': {
                  backgroundColor: 'rgba(255,255,255,0.15)',
                  '&:hover': { backgroundColor: 'rgba(255,255,255,0.2)' },
                },
                '&:hover': { backgroundColor: 'rgba(255,255,255,0.1)' },
              }}
            >
              <ListItemIcon sx={{ minWidth: 0, mr: open ? 2 : 'auto', justifyContent: 'center', color: '#fff' }}>
                {item.icon}
              </ListItemIcon>
              {open && <ListItemText primary={item.text} />}
            </ListItemButton>
          </ListItem>
        ))}
      </List>

      <Box sx={{ flexGrow: 1 }} />

      <Divider sx={{ borderColor: 'rgba(255,255,255,0.1)' }} />

      <List>
        <ListItem disablePadding>
          <ListItemButton
            onClick={() => {
              localStorage.clear();
              window.location.href = '/login';
            }}
            sx={{
              minHeight: 48,
              justifyContent: open ? 'initial' : 'center',
              px: 2.5,
              '&:hover': { backgroundColor: 'rgba(255,255,255,0.1)' },
            }}
          >
            <ListItemIcon sx={{ minWidth: 0, mr: open ? 2 : 'auto', justifyContent: 'center', color: '#fff' }}>
              <Logout />
            </ListItemIcon>
            {open && <ListItemText primary="Salir" />}
          </ListItemButton>
        </ListItem>
      </List>
    </Drawer>
  );
}