import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../services/api';

export const fetchCamere = createAsyncThunk('camere/fetchCamere', async () => {
  const res = await api.get('/camere');
  return res.data;
});

const camereSlice = createSlice({
  name: 'camere',
  initialState: {
    camere: [],
    loading: false,
    error: null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchCamere.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchCamere.fulfilled, (state, action) => {
        state.loading = false;
        state.camere = action.payload;
      })
      .addCase(fetchCamere.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message;
      });
  },
});

export default camereSlice.reducer;