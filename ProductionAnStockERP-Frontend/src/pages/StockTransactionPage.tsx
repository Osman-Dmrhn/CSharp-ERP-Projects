import React, { useState } from 'react';
import stockTransactionService from '../api/stockTransactionService';  // API servis dosyanız
import type { StockTransactionCreateDto } from '../models/StockTransactionDtos/StockTransactionCreateDto';

const emptyStockTransaction: StockTransactionCreateDto = {
  productName: '',
  quantity: 1,
  transactionType: 'Entry',  
  relatedOrderId: null,      
};

const StockTransactionPage: React.FC = () => {
  const [newStockTransaction, setNewStockTransaction] = useState<StockTransactionCreateDto>(emptyStockTransaction);
  const [loading, setLoading] = useState(false);

  const handleCreate = async () => {
    setLoading(true);
    try {
      const result = await stockTransactionService.createStockTransaction(newStockTransaction);
      if (result.success) {
        alert('Stok işlemi başarıyla oluşturuldu!');
        setNewStockTransaction(emptyStockTransaction);  
      } else {
        alert('Bir hata oluştu.');
      }
    } catch (error) {
      console.error('Stok işlemi oluşturulurken hata:', error);
      alert('Bir hata oluştu.');
    }
    setLoading(false);
  };

  return (
    <div className="container-fluid min-h-screen bg-light py-5 px-3">
      <div className="row justify-content-center">
        <div className="col-lg-10 col-xl-9">
          {/* Yeni Stok İşlemi Formu */}
          <div className="bg-white rounded shadow p-4 mb-5">
            <h2 className="text-primary mb-4">Yeni Stok İşlemi Oluştur</h2>
            <div className="row g-3">
              <div className="col-md-6">
                <input
                  type="text"
                  placeholder="Ürün Adı"
                  value={newStockTransaction.productName}
                  onChange={(e) =>
                    setNewStockTransaction({ ...newStockTransaction, productName: e.target.value })
                  }
                  className="form-control"
                />
              </div>
              <div className="col-md-6">
                <input
                  type="number"
                  placeholder="Miktar"
                  value={newStockTransaction.quantity}
                  onChange={(e) =>
                    setNewStockTransaction({ ...newStockTransaction, quantity: parseInt(e.target.value) })
                  }
                  className="form-control"
                />
              </div>
              <div className="col-md-6">
                <select
                  className="form-control"
                  value={newStockTransaction.transactionType}
                  onChange={(e) =>
                    setNewStockTransaction({
                      ...newStockTransaction,
                      transactionType: e.target.value as 'Entry' | 'Exit',
                    })
                  }
                >
                  <option value="Entry">Giriş</option>
                  <option value="Exit">Çıkış</option>
                </select>
              </div>
              <div className="col-md-6">
                <input
                  type="number"
                  placeholder="İlgili Sipariş ID (Opsiyonel)"
                  value={newStockTransaction.relatedOrderId || ''}
                  onChange={(e) =>
                    setNewStockTransaction({ ...newStockTransaction, relatedOrderId: parseInt(e.target.value) || null })
                  }
                  className="form-control"
                />
              </div>
            </div>
            <div className="mt-3 text-end">
              <button onClick={handleCreate} className="btn btn-primary" disabled={loading}>
                {loading ? 'Yükleniyor...' : 'Ekle'}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default StockTransactionPage;
