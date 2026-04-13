import { Box, Typography, Paper, Card, CardContent } from '@mui/material';
import { Inventory, Link as LinkIcon, TrendingUp } from '@mui/icons-material';

export default function Dashboard() {
  const stats = [
    { title: 'Total Recursos', value: '124', icon: <Inventory sx={{ fontSize: 40, color: '#1976d2' }} /> },
    { title: 'URLs Creadas', value: '89', icon: <LinkIcon sx={{ fontSize: 40, color: '#2e7d32' }} /> },
    { title: 'Clics Totales', value: '1,234', icon: <TrendingUp sx={{ fontSize: 40, color: '#ed6c02' }} /> },
  ];

  return (
    <Box>
      <Typography variant="h4" sx={{ mb: 3, fontWeight: 600, color: '#1a1a2e' }}>
        Resumen General
      </Typography>

      <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 3 }}>
        {stats.map((stat, index) => (
          <Box key={index} sx={{ flex: '1 1 300px', maxWidth: { xs: '100%', md: 'calc(33.333% - 24px)' } }}>
            <Card sx={{ borderRadius: 2, boxShadow: '0 2px 8px rgba(0,0,0,0.08)' }}>
              <CardContent sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                {stat.icon}
                <Box>
                  <Typography variant="h4" sx={{ fontWeight: 700 }}>{stat.value}</Typography>
                  <Typography variant="body2" color="text.secondary">{stat.title}</Typography>
                </Box>
              </CardContent>
            </Card>
          </Box>
        ))}
      </Box>

      <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 3, mt: 2 }}>
        <Box sx={{ flex: '1 1 400px' }}>
          <Paper sx={{ p: 3, borderRadius: 2 }}>
            <Typography variant="h6" sx={{ mb: 2 }}>Actividad Reciente</Typography>
            <Typography color="text.secondary">No hay actividad reciente</Typography>
          </Paper>
        </Box>
        <Box sx={{ flex: '1 1 400px' }}>
          <Paper sx={{ p: 3, borderRadius: 2 }}>
            <Typography variant="h6" sx={{ mb: 2 }}>Estadísticas</Typography>
            <Typography color="text.secondary">Sin datos disponibles</Typography>
          </Paper>
        </Box>
      </Box>
    </Box>
  );
}