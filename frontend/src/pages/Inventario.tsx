import { useState, useEffect } from 'react';
import {
  Box, Typography, Paper, Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Button, IconButton, Chip, Dialog, DialogTitle,
  DialogContent, DialogActions, TextField, Alert, Snackbar, ToggleButtonGroup, ToggleButton, MenuItem
} from '@mui/material';
import { Add, Edit, Delete, Remove, Add as AddIcon, Warning, CheckCircle, Schedule } from '@mui/icons-material';
import { inventarioApi } from '../api/inventario';
import { RecursoDTO, CreateRecursoRequest, PrioridadLabels, EstadoLabels, PrioridadColors, EstadoColors } from '../types/inventario';

type Vista = 'tareas' | 'inventario';

export default function Inventario() {
  const [recursos, setRecursos] = useState<RecursoDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [vista, setVista] = useState<Vista>('tareas');
  const [openDialog, setOpenDialog] = useState(false);
  const [openStockDialog, setOpenStockDialog] = useState(false);
  const [editingRecurso, setEditingRecurso] = useState<RecursoDTO | null>(null);
  const [selectedRecurso, setSelectedRecurso] = useState<RecursoDTO | null>(null);
  const [stockAction, setStockAction] = useState<'agregar' | 'descontar'>('agregar');
  const [stockAmount, setStockAmount] = useState(1);
  const [formData, setFormData] = useState<CreateRecursoRequest>({
    nombre: '',
    descripcion: '',
    stockInicial: 0,
    umbralMinimo: 0,
    prioridad: 1
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  const fetchRecursos = async () => {
    try {
      setLoading(true);
      const data = await inventarioApi.getAll();
      setRecursos(data);
    } catch (err) {
      showSnackbar('Error al cargar recursos', 'error');
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

  const filteredRecursos = recursos.filter(r => {
    if (vista === 'tareas') {
      return r.stock === 0 && !r.codigoCorto;
    } else {
      return r.stock > 0 || r.codigoCorto;
    }
  });

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
        stockInicial: vista === 'inventario' ? 0 : 0,
        umbralMinimo: vista === 'inventario' ? 0 : 0,
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
    try {
      if (editingRecurso) {
        await inventarioApi.update(editingRecurso.id, formData);
        showSnackbar('Actualizado correctamente', 'success');
      } else {
        await inventarioApi.create(formData);
        showSnackbar('Creado correctamente', 'success');
      }
      handleCloseDialog();
      fetchRecursos();
    } catch (err) {
      showSnackbar(err instanceof Error ? err.message : 'Error al guardar', 'error');
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('¿Estás seguro de eliminar?')) return;
    try {
      await inventarioApi.delete(id);
      showSnackbar('Eliminado correctamente', 'success');
      fetchRecursos();
    } catch (err) {
      showSnackbar('Error al eliminar', 'error');
    }
  };

  const handleOpenStockDialog = (recurso: RecursoDTO, action: 'agregar' | 'descontar') => {
    setSelectedRecurso(recurso);
    setStockAction(action);
    setStockAmount(1);
    setOpenStockDialog(true);
  };

  const handleStockAction = async () => {
    if (!selectedRecurso) return;
    try {
      if (stockAction === 'agregar') {
        await inventarioApi.agregarStock(selectedRecurso.id, stockAmount);
        showSnackbar('Stock agregado', 'success');
      } else {
        await inventarioApi.descontarStock(selectedRecurso.id, stockAmount);
        showSnackbar('Stock descontado', 'success');
      }
      setOpenStockDialog(false);
      fetchRecursos();
    } catch (err) {
      showSnackbar(err instanceof Error ? err.message : 'Error', 'error');
    }
  };

  const handleCambiarEstado = async (recurso: RecursoDTO, nuevoEstado: number) => {
    try {
      if (nuevoEstado === 2) {
        await inventarioApi.descontarStock(recurso.id, 0);
      }
      await inventarioApi.update(recurso.id, {
        ...formData,
        nombre: recurso.nombre,
        descripcion: recurso.descripcion,
        stockInicial: recurso.stock,
        umbralMinimo: recurso.umbralMinimo,
        prioridad: recurso.prioridad
      });
      fetchRecursos();
    } catch (err) {
      showSnackbar('Error al cambiar estado', 'error');
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" sx={{ fontWeight: 600, color: '#1a1a2e' }}>
          {vista === 'tareas' ? 'Gestor de Tareas' : 'Gestión de Inventario'}
        </Typography>
        <Button
          variant="contained"
          startIcon={<Add />}
          onClick={() => handleOpenDialog()}
          sx={{ backgroundColor: '#1a1a2e', '&:hover': { backgroundColor: '#2d2d44' } }}
        >
          Nueva {vista === 'tareas' ? 'Tarea' : 'Recurso'}
        </Button>
      </Box>

      <Box sx={{ mb: 3 }}>
        <ToggleButtonGroup
          value={vista}
          exclusive
          onChange={(_, v) => v && setVista(v)}
          sx={{ '& .Mui-selected': { backgroundColor: '#1a1a2e', color: '#fff' } }}
        >
          <ToggleButton value="tareas">
            <Schedule sx={{ mr: 1 }} /> Tareas
          </ToggleButton>
          <ToggleButton value="inventario">
            <Warning sx={{ mr: 1 }} /> Inventario
          </ToggleButton>
        </ToggleButtonGroup>
      </Box>

      <TableContainer component={Paper} sx={{ borderRadius: 2 }}>
        <Table>
          <TableHead sx={{ backgroundColor: '#f5f5f5' }}>
            <TableRow>
              <TableCell sx={{ fontWeight: 600 }}>ID</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>{vista === 'tareas' ? 'Título' : 'Nombre'}</TableCell>
              {vista === 'tareas' ? (
                <>
                  <TableCell sx={{ fontWeight: 600 }}>Descripción</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Prioridad</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Estado</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Vencimiento</TableCell>
                </>
              ) : (
                <>
                  <TableCell sx={{ fontWeight: 600 }}>Stock</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Umbral</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Clicks</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Estado</TableCell>
                </>
              )}
              <TableCell sx={{ fontWeight: 600 }}>Acciones</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={8} align="center">Cargando...</TableCell>
              </TableRow>
            ) : filteredRecursos.length === 0 ? (
              <TableRow>
                <TableCell colSpan={8} align="center">
                  No hay {vista === 'tareas' ? 'tareas' : 'recursos'} registrados
                </TableCell>
              </TableRow>
            ) : (
              filteredRecursos.map((recurso) => (
                <TableRow key={recurso.id} hover>
                  <TableCell>{recurso.id}</TableCell>
                  <TableCell sx={{ fontWeight: 500 }}>{recurso.nombre}</TableCell>
                  {vista === 'tareas' ? (
                    <>
                      <TableCell>{recurso.descripcion || '-'}</TableCell>
                      <TableCell>
                        <Chip 
                          label={PrioridadLabels[recurso.prioridad]} 
                          color={PrioridadColors[recurso.prioridad]} 
                          size="small" 
                        />
                      </TableCell>
                      <TableCell>
                        <Chip 
                          label={EstadoLabels[recurso.estado]} 
                          color={EstadoColors[recurso.estado]} 
                          size="small" 
                          variant={recurso.estado === 2 ? 'filled' : 'outlined'}
                        />
                      </TableCell>
                      <TableCell>
                        {recurso.fechaVencimiento ? (
                          recurso.estaVencida ? (
                            <Chip label="Vencida" color="error" size="small" />
                          ) : (
                            new Date(recurso.fechaVencimiento).toLocaleDateString()
                          )
                        ) : '-'}
                      </TableCell>
                    </>
                  ) : (
                    <>
                      <TableCell>
                        <Chip
                          label={recurso.stock}
                          color={recurso.estaEnEstadoCritico ? 'error' : 'default'}
                          size="small"
                        />
                      </TableCell>
                      <TableCell>{recurso.umbralMinimo}</TableCell>
                      <TableCell>{recurso.clicks}</TableCell>
                      <TableCell>
                        {recurso.estaEnEstadoCritico ? (
                          <Chip icon={<Warning />} label="Crítico" color="error" size="small" />
                        ) : (
                          <Chip label="Normal" color="success" size="small" variant="outlined" />
                        )}
                      </TableCell>
                    </>
                  )}
                  <TableCell>
                    {vista === 'inventario' && (
                      <>
                        <IconButton size="small" onClick={() => handleOpenStockDialog(recurso, 'agregar')}>
                          <AddIcon fontSize="small" />
                        </IconButton>
                        <IconButton size="small" onClick={() => handleOpenStockDialog(recurso, 'descontar')}>
                          <Remove fontSize="small" />
                        </IconButton>
                      </>
                    )}
                    {vista === 'tareas' && recurso.estado !== 2 && (
                      <IconButton size="small" onClick={() => handleCambiarEstado(recurso, 2)} color="success">
                        <CheckCircle fontSize="small" />
                      </IconButton>
                    )}
                    <IconButton size="small" onClick={() => handleOpenDialog(recurso)}>
                      <Edit fontSize="small" />
                    </IconButton>
                    <IconButton size="small" onClick={() => handleDelete(recurso.id)} color="error">
                      <Delete fontSize="small" />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
        <DialogTitle>
          {editingRecurso ? 'Editar' : 'Nuevo'} {vista === 'tareas' ? 'Tarea' : 'Recurso'}
        </DialogTitle>
        <DialogContent>
          <TextField fullWidth label="Nombre" value={formData.nombre} onChange={(e) => setFormData({ ...formData, nombre: e.target.value })} margin="normal" />
          
          {vista === 'tareas' && (
            <TextField fullWidth label="Descripción" value={formData.descripcion} onChange={(e) => setFormData({ ...formData, descripcion: e.target.value })} margin="normal" multiline rows={2} />
          )}
          
          {vista === 'inventario' && (
            <>
              <TextField fullWidth label="Stock" type="number" value={formData.stockInicial} onChange={(e) => setFormData({ ...formData, stockInicial: parseInt(e.target.value) || 0 })} margin="normal" />
              <TextField fullWidth label="Umbral Mínimo" type="number" value={formData.umbralMinimo} onChange={(e) => setFormData({ ...formData, umbralMinimo: parseInt(e.target.value) || 0 })} margin="normal" />
            </>
          )}
          
          {vista === 'tareas' && (
            <TextField
              fullWidth
              label="Fecha Vencimiento"
              type="datetime-local"
              value={formData.fechaVencimiento || ''}
              onChange={(e) => setFormData({ ...formData, fechaVencimiento: e.target.value || undefined })}
              margin="normal"
              slotProps={{ inputLabel: { shrink: true } }}
            />
          )}
          
          {vista === 'tareas' && (
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
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>Cancelar</Button>
          <Button onClick={handleSave} variant="contained" sx={{ backgroundColor: '#1a1a2e' }}>Guardar</Button>
        </DialogActions>
      </Dialog>

      <Dialog open={openStockDialog} onClose={() => setOpenStockDialog(false)} maxWidth="xs" fullWidth>
        <DialogTitle>{stockAction === 'agregar' ? 'Agregar Stock' : 'Descontar Stock'}</DialogTitle>
        <DialogContent>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Recurso: {selectedRecurso?.nombre} | Stock: {selectedRecurso?.stock}
          </Typography>
          <TextField fullWidth label="Cantidad" type="number" value={stockAmount} onChange={(e) => setStockAmount(Math.max(1, parseInt(e.target.value) || 1))} slotProps={{ htmlInput: { min: 1 } }} />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenStockDialog(false)}>Cancelar</Button>
          <Button onClick={handleStockAction} variant="contained" sx={{ backgroundColor: '#1a1a2e' }}>{stockAction === 'agregar' ? 'Agregar' : 'Descontar'}</Button>
        </DialogActions>
      </Dialog>

      <Snackbar open={snackbar.open} autoHideDuration={4000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity} onClose={() => setSnackbar({ ...snackbar, open: false })}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
}