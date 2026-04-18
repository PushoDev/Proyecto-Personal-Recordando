import { useState, useEffect } from 'react';
import {
  Box, Typography, Paper, Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Button, IconButton, Chip, Dialog, DialogTitle,
  DialogContent, DialogActions, TextField, Alert, Snackbar, MenuItem,
  DialogContentText, Stack, Card, CardContent, CardActions
} from '@mui/material';
import { Add, Edit, Delete, CheckCircle, TaskAlt, DeleteForever } from '@mui/icons-material';
import { inventarioApi } from '../api/inventario';
import { RecursoDTO, CreateRecursoRequest, PrioridadLabels, EstadoLabels, PrioridadColors, EstadoColors } from '../types/inventario';

export default function Inventario() {
  const [recursos, setRecursos] = useState<RecursoDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingRecurso, setEditingRecurso] = useState<RecursoDTO | null>(null);
  const [formData, setFormData] = useState<CreateRecursoRequest>({
    nombre: '',
    descripcion: '',
    stockInicial: 0,
    umbralMinimo: 0,
    prioridad: 1
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });
  const [openConfirmComplete, setOpenConfirmComplete] = useState(false);
  const [recursoToComplete, setRecursoToComplete] = useState<RecursoDTO | null>(null);
  const [openConfirmDelete, setOpenConfirmDelete] = useState(false);
  const [recursoToDelete, setRecursoToDelete] = useState<{id: number, nombre: string} | null>(null);
  const [page, setPage] = useState(0);
  const [rowsPerPage] = useState(5);

  const fetchRecursos = async () => {
    try {
      setLoading(true);
      const data = await inventarioApi.getAll();
      setRecursos(data);
    } catch (err) {
      showSnackbar('Error al cargar tareas', 'error');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRecursos();
  }, []);

  const showSnackbar = (message: string, severity: 'success' | 'error') => {
    setSnackbar({ open: true, message, severity });
  };

  const tareas = recursos.filter(r => r.stock === 0 && !r.codigoCorto);
  const paginatedTareas = tareas.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);

  const handleOpenDialog = (recurso?: RecursoDTO) => {
    if (recurso) {
      setEditingRecurso(recurso);
      setFormData({
        nombre: recurso.nombre,
        descripcion: recurso.descripcion || '',
        stockInicial: recurso.stock,
        umbralMinimo: recurso.umbralMinimo,
        prioridad: recurso.prioridad
      });
    } else {
      setEditingRecurso(null);
      setFormData({
        nombre: '',
        descripcion: '',
        stockInicial: 0,
        umbralMinimo: 0,
        prioridad: 1
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingRecurso(null);
  };

  const handleSave = async () => {
    if (!formData.nombre.trim()) {
      showSnackbar('El título es requerido', 'error');
      return;
    }
    const dataToSend = {
      nombre: formData.nombre,
      descripcion: formData.descripcion || null,
      stockInicial: 0,
      umbralMinimo: 0,
      prioridad: formData.prioridad,
      fechaVencimiento: formData.fechaVencimiento || null
    };
    console.log('Enviando:', dataToSend);
    try {
      if (editingRecurso) {
        await inventarioApi.update(editingRecurso.id, dataToSend);
        showSnackbar('Actualizado correctamente', 'success');
      } else {
        await inventarioApi.create(dataToSend);
        showSnackbar('Creado correctamente', 'success');
      }
      handleCloseDialog();
      fetchRecursos();
    } catch (err) {
      showSnackbar(err instanceof Error ? err.message : 'Error al guardar', 'error');
    }
  };

  const handleDelete = async () => {
    if (!recursoToDelete) return;
    try {
      await inventarioApi.delete(recursoToDelete.id);
      showSnackbar('Eliminado correctamente', 'success');
      setOpenConfirmDelete(false);
      fetchRecursos();
    } catch (err) {
      showSnackbar('Error al eliminar', 'error');
      setOpenConfirmDelete(false);
    }
  };

  const confirmDelete = (id: number, nombre: string) => {
    setRecursoToDelete({ id, nombre });
    setOpenConfirmDelete(true);
  };

  const handleCompletar = async () => {
    if (!recursoToComplete) return;
    try {
      await inventarioApi.update(recursoToComplete.id, {
        nombre: recursoToComplete.nombre,
        descripcion: recursoToComplete.descripcion,
        stockInicial: 0,
        umbralMinimo: 0,
        prioridad: recursoToComplete.prioridad,
        fechaVencimiento: recursoToComplete.fechaVencimiento || null,
        estado: 2
      });
      showSnackbar('Tarea completada', 'success');
      setOpenConfirmComplete(false);
      fetchRecursos();
    } catch (err) {
      showSnackbar('Error al completar tarea', 'error');
      setOpenConfirmComplete(false);
    }
  };

  const confirmComplete = (recurso: RecursoDTO) => {
    setRecursoToComplete(recurso);
    setOpenConfirmComplete(true);
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" sx={{ fontWeight: 600, color: '#1a1a2e' }}>
          Mis Tareas
        </Typography>
        <Button
          variant="contained"
          startIcon={<Add />}
          onClick={() => handleOpenDialog()}
          sx={{ backgroundColor: '#1a1a2e', '&:hover': { backgroundColor: '#2d2d44' } }}
        >
          Nueva Tarea
        </Button>
      </Box>

      <TableContainer sx={{ borderRadius: 2, boxShadow: '0 2px 12px rgba(0,0,0,0.08)' }}>
        <Table>
          <TableHead>
            <TableRow sx={{ backgroundColor: '#1a1a2e' }}>
              <TableCell sx={{ color: 'white', fontWeight: 600 }}>#</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 600 }}>Tarea</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 600 }}>Descripción</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 600 }}>Prioridad</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 600 }}>Estado</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 600 }}>Vence</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 600, textAlign: 'center' }}>Acciones</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={7} align="center" sx={{ py: 4 }}>Cargando...</TableCell>
              </TableRow>
            ) : paginatedTareas.length === 0 ? (
              <TableRow>
                <TableCell colSpan={7} align="center" sx={{ py: 4, color: 'text.secondary' }}>No hay tareas</TableCell>
              </TableRow>
            ) : (
              paginatedTareas.map((tarea, index) => (
                <TableRow 
                  key={tarea.id} 
                  hover
                  sx={{ 
                    '&:nth-of-type(odd)': { backgroundColor: '#f8f9fa' },
                    '&:hover': { backgroundColor: '#e3f2fd' },
                    transition: 'background-color 0.2s'
                  }}
                >
                  <TableCell sx={{ fontWeight: 500, color: '#666' }}>{page * rowsPerPage + index + 1}</TableCell>
                  <TableCell>
                    <Typography variant="body2" sx={{ fontWeight: 600, textDecoration: tarea.estado === 2 ? 'line-through' : 'none', color: tarea.estado === 2 ? 'text.disabled' : 'text.primary' }}>
                      {tarea.nombre}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2" color="text.secondary" sx={{ maxWidth: 200, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                      {tarea.descripcion || '-'}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Chip 
                      label={PrioridadLabels[tarea.prioridad]} 
                      color={PrioridadColors[tarea.prioridad]} 
                      size="small" 
                      sx={{ fontWeight: 500 }}
                    />
                  </TableCell>
                  <TableCell>
                    <Chip 
                      label={EstadoLabels[tarea.estado]} 
                      color={EstadoColors[tarea.estado]} 
                      size="small" 
                      variant={tarea.estado === 2 ? 'filled' : 'outlined'}
                    />
                  </TableCell>
                  <TableCell>
                    {tarea.fechaVencimiento ? (
                      <Chip 
                        label={tarea.estaVencida ? 'Vencida' : new Date(tarea.fechaVencimiento).toLocaleDateString()} 
                        color={tarea.estaVencida ? 'error' : 'default'} 
                        size="small" 
                      />
                    ) : '-'}
                  </TableCell>
                  <TableCell align="center">
                    <Stack direction="row" justifyContent="center" spacing={0.5}>
                      {tarea.estado !== 2 && (
                        <IconButton size="small" onClick={() => confirmComplete(tarea)} color="success" sx={{ '&:hover': { backgroundColor: 'rgba(76, 175, 80, 0.1)' } }}>
                          <CheckCircle fontSize="small" />
                        </IconButton>
                      )}
                      <IconButton size="small" onClick={() => handleOpenDialog(tarea)} sx={{ '&:hover': { backgroundColor: 'rgba(25, 118, 210, 0.1)' } }}>
                        <Edit fontSize="small" />
                      </IconButton>
                      <IconButton size="small" onClick={() => confirmDelete(tarea.id, tarea.nombre)} color="error" sx={{ '&:hover': { backgroundColor: 'rgba(244, 67, 54, 0.1)' } }}>
                        <Delete fontSize="small" />
                      </IconButton>
                    </Stack>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <Stack direction="row" justifyContent="center" spacing={2} sx={{ mt: 3, mb: 2 }}>
        <Button 
          variant="outlined" 
          disabled={page === 0}
          onClick={() => setPage(page - 1)}
          sx={{ minWidth: 100 }}
        >
          Anterior
        </Button>
        <Button 
          variant="outlined"
          disabled={page >= Math.ceil(tareas.length / rowsPerPage) - 1}
          onClick={() => setPage(page + 1)}
          sx={{ minWidth: 100 }}
        >
          Siguiente
        </Button>
      </Stack>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
        <DialogTitle>
          {editingRecurso ? 'Editar Tarea' : 'Nueva Tarea'}
        </DialogTitle>
        <DialogContent>
          <TextField 
            fullWidth 
            label="Título" 
            value={formData.nombre} 
            onChange={(e) => setFormData({ ...formData, nombre: e.target.value })} 
            margin="normal" 
          />
          
          <TextField 
            fullWidth 
            label="Descripción" 
            value={formData.descripcion} 
            onChange={(e) => setFormData({ ...formData, descripcion: e.target.value })} 
            margin="normal" 
            multiline 
            rows={2} 
          />
          
          <TextField
            fullWidth
            label="Fecha Vencimiento"
            type="datetime-local"
            value={formData.fechaVencimiento || ''}
            onChange={(e) => setFormData({ ...formData, fechaVencimiento: e.target.value || undefined })}
            margin="normal"
            slotProps={{ inputLabel: { shrink: true } }}
          />
          
          <TextField
            fullWidth
            select
            label="Prioridad"
            value={formData.prioridad}
            onChange={(e) => setFormData({ ...formData, prioridad: parseInt(e.target.value) })}
            margin="normal"
          >
            <MenuItem value={0}>Baja</MenuItem>
            <MenuItem value={1}>Media</MenuItem>
            <MenuItem value={2}>Alta</MenuItem>
          </TextField>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>Cancelar</Button>
          <Button onClick={handleSave} variant="contained" sx={{ backgroundColor: '#1a1a2e' }}>Guardar</Button>
        </DialogActions>
      </Dialog>

      <Dialog open={openConfirmDelete} onClose={() => setOpenConfirmDelete(false)}>
        <DialogTitle sx={{ display: 'flex', alignItems: 'center', gap: 1, color: 'error.main' }}>
          <DeleteForever color="error" /> Eliminar Tarea
        </DialogTitle>
        <DialogContent>
          <DialogContentText>
            ¿Eliminar "{recursoToDelete?.nombre}"? Esta acción no se puede deshacer.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenConfirmDelete(false)}>Cancelar</Button>
          <Button onClick={handleDelete} variant="contained" color="error" startIcon={<DeleteForever />}>
            Eliminar
          </Button>
        </DialogActions>
      </Dialog>

      <Dialog open={openConfirmComplete} onClose={() => setOpenConfirmComplete(false)}>
        <DialogTitle sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <TaskAlt color="success" /> Completar Tarea
        </DialogTitle>
        <DialogContent>
          <DialogContentText>
            ¿Marcas "{recursoToComplete?.nombre}" como completada?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenConfirmComplete(false)}>Cancelar</Button>
          <Button onClick={handleCompletar} variant="contained" color="success" startIcon={<TaskAlt />}>
            Completar
          </Button>
        </DialogActions>
      </Dialog>

      <Snackbar 
        open={snackbar.open} 
        autoHideDuration={4000} 
        onClose={() => setSnackbar({ ...snackbar, open: false })}
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
      >
        <Alert 
          severity={snackbar.severity} 
          onClose={() => setSnackbar({ ...snackbar, open: false })}
          variant="filled"
          sx={{ 
            width: '100%',
            fontWeight: 500,
            boxShadow: '0 4px 12px rgba(0,0,0,0.15)',
            '&.MuiAlert-filledSuccess': { backgroundColor: '#2e7d32' },
            '&.MuiAlert-filledError': { backgroundColor: '#d32f2f' },
            '&.MuiAlert-filledWarning': { backgroundColor: '#ed6c02' }
          }}
        >
          {snackbar.message}
        </Alert>
      </Snackbar>
    </Box>
  );
}