import { Box, Typography, Paper } from '@mui/material';

export default function UrlShortener() {
  return (
    <Box>
      <Typography variant="h4" sx={{ mb: 3, fontWeight: 600, color: '#1a1a2e' }}>
        Acortador de URLs
      </Typography>
      <Paper sx={{ p: 3, borderRadius: 2 }}>
        <Typography color="text.secondary">
          Módulo de URL shortener - En desarrollo
        </Typography>
      </Paper>
    </Box>
  );
}