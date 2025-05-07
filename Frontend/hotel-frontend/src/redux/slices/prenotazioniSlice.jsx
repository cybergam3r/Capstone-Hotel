import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../services/api';

export const fetchMiePrenotazioni = createAsyncThunk(
  'prenotazioni/fetchMie',
  async () => {
    const res = await api.get('/prenotazioni/mie');
    return res.data;
  }
);

const prenotazioniSlice = createSlice({
  name: 'prenotazioni',
  initialState: {
    miePrenotazioni: [],
    loading: false,
    error: null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchMiePrenotazioni.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchMiePrenotazioni.fulfilled, (state, action) => {
        state.loading = false;
        state.miePrenotazioni = action.payload;
      })
      .addCase(fetchMiePrenotazioni.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message;
      });
  },
});

export default prenotazioniSlice.reducer;