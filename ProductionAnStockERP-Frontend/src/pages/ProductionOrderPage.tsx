import React, { useEffect, useState } from 'react';
import productionOrderService from '../api/productionOrderService'; 
import OrderService from '../api/OrderService'; 
import type { ProductionOrder } from '../models/ProductionOrderDtos/ProductionOrder';
import type { ProductionOrderCreateDto } from '../models/ProductionOrderDtos/ProductionOrderCreateDto';
import type { ProductionOrderUpdateDto } from '../models/ProductionOrderDtos/ProductionOrderUpdateDto';
import type { Order } from '../models/OrderDtos/Order';
import type{ ProductionOrderStatus } from '../models/ProductionOrderDtos/ProductionOrderStatus';  

const emptyProductionOrder: ProductionOrderCreateDto = {
  productName: '',
  quantity: 1,
};

const emptyUpdateProductionOrder: ProductionOrderUpdateDto = {
  productName: '',
  quantity: 1,
  status: 'InProgress',  
};

const ProductionOrderPage: React.FC = () => {
  const [productionOrders, setProductionOrders] = useState<ProductionOrder[]>([]);
  const [orders, setOrders] = useState<Order[]>([]);  // Siparişleri tutacak
  const [loading, setLoading] = useState(true);
  const [newProductionOrder, setNewProductionOrder] = useState<ProductionOrderCreateDto>(emptyProductionOrder);
  const [editProductionOrderId, setEditProductionOrderId] = useState<number | null>(null);
  const [editFormData, setEditFormData] = useState<ProductionOrderUpdateDto>(emptyUpdateProductionOrder);

  useEffect(() => {
    fetchProductionOrders();
    fetchOrders();  // Siparişleri çekiyoruz
  }, []);

  const fetchProductionOrders = async () => {
    const result = await productionOrderService.getAllProductionOrders();
    if (result.success) {
      setProductionOrders(result.data);
    }
    setLoading(false);
  };

  const fetchOrders = async () => {
    const result = await OrderService.getAllOrders();
    if (result.success) {
      setOrders(result.data); 
    }
  };

  const handleCreate = async () => {
    const result = await productionOrderService.createProductionOrder(newProductionOrder);
    if (result.success) {
      fetchProductionOrders();
      setNewProductionOrder(emptyProductionOrder);
    }
  };

  const handleDelete = async (id: number) => {
    const result = await productionOrderService.deleteProductionOrder(id);
    if (result.success) {
      fetchProductionOrders();
    }
  };

  const handleUpdate = async () => {
    if (editProductionOrderId !== null) {
      const result = await productionOrderService.updateProductionOrder(editProductionOrderId, editFormData);
      if (result.success) {
        fetchProductionOrders();
        setEditProductionOrderId(null);
      }
    }
  };

  const handleEditClick = (order: ProductionOrder) => {
    setEditProductionOrderId(order.productionId);
    setEditFormData({
      productName: order.productName,
      quantity: order.quantity,
      status: order.status,
    });
  };

  return (
    <div className="container-fluid min-h-screen bg-light py-5 px-3">
      <div className="row justify-content-center">
        <div className="col-lg-10 col-xl-9">
          {/* Yeni Üretim Siparişi Formu */}
          <div className="bg-white rounded shadow p-4 mb-5">
            <h2 className="text-primary mb-4">Yeni Üretim Siparişi Oluştur</h2>
            <div className="row g-3">
              <div className="col-md-6">
                <input
                  type="text"
                  placeholder="Ürün Adı"
                  value={newProductionOrder.productName}
                  onChange={(e) =>
                    setNewProductionOrder({ ...newProductionOrder, productName: e.target.value })
                  }
                  className="form-control"
                />
              </div>
              <div className="col-md-6">
                <input
                  type="number"
                  placeholder="Miktar"
                  value={newProductionOrder.quantity}
                  onChange={(e) =>
                    setNewProductionOrder({ ...newProductionOrder, quantity: parseInt(e.target.value) })
                  }
                  className="form-control"
                />
              </div>
              <div className="col-md-6">
                <select
                  className="form-control"
                  value={newProductionOrder.orderId || ''}
                  onChange={(e) =>
                    setNewProductionOrder({ ...newProductionOrder, orderId: parseInt(e.target.value) || null })
                  }
                >
                  <option value="">Bir Sipariş Seçin</option>
                  {orders.map((order) => (
                    <option key={order.orderId} value={order.orderId}>
                      {order.productName} - {order.customerName}
                    </option>
                  ))}
                </select>
              </div>
            </div>
            <div className="mt-3 text-end">
              <button onClick={handleCreate} className="btn btn-primary">
                Ekle
              </button>
            </div>
          </div>

          {/* Üretim Sipariş Listesi */}
          <div className="bg-white rounded shadow p-4">
            <h3 className="text-dark mb-3">Üretim Sipariş Listesi</h3>
            {loading ? (
              <p>Yükleniyor...</p>
            ) : (
              <div className="table-responsive">
                <table className="table table-bordered table-hover">
                  <thead className="table-light">
                    <tr>
                      <th>Ürün Adı</th>
                      <th>Miktar</th>
                      <th>Durum</th>
                      <th className="text-center">İşlemler</th>
                    </tr>
                  </thead>
                  <tbody>
                    {productionOrders.map((order) => (
                      <tr key={order.productionId}>
                        <td>{order.productName}</td>
                        <td>{order.quantity}</td>
                        <td>{order.status}</td>
                        <td className="text-center">
                          <button
                            className="btn btn-warning btn-sm me-2"
                            onClick={() => handleEditClick(order)}
                          >
                            Güncelle
                          </button>
                          <button
                            className="btn btn-danger btn-sm"
                            onClick={() => handleDelete(order.productionId)}
                          >
                            Sil
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Güncelleme Paneli */}
      {editProductionOrderId !== null && (
        <div className="position-fixed top-0 start-0 w-100 h-100 bg-dark bg-opacity-50 d-flex justify-content-center align-items-center zindex-tooltip">
          <div className="bg-white rounded shadow p-4 w-100" style={{ maxWidth: '500px' }}>
            <h4 className="mb-4">Üretim Siparişi Güncelle</h4>
            <div className="mb-3">
              <label className="form-label">Ürün Adı</label>
              <input
                type="text"
                value={editFormData.productName}
                onChange={(e) =>
                  setEditFormData({ ...editFormData, productName: e.target.value })
                }
                className="form-control"
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Miktar</label>
              <input
                type="number"
                value={editFormData.quantity}
                onChange={(e) =>
                  setEditFormData({ ...editFormData, quantity: parseInt(e.target.value) })
                }
                className="form-control"
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Durum</label>
              <select
                className="form-control"
                value={editFormData.status}
                onChange={(e) =>
                  setEditFormData({ ...editFormData, status: e.target.value as ProductionOrderStatus })
                }
              >
                <option value="InProgress">Devam Ediyor</option>
                <option value="Started">Başladı</option>
                <option value="Completed">Tamamlandı</option>
              </select>
            </div>
            <div className="text-end">
              <button
                className="btn btn-secondary me-2"
                onClick={() => setEditProductionOrderId(null)}
              >
                İptal
              </button>
              <button className="btn btn-primary" onClick={handleUpdate}>
                Kaydet
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductionOrderPage;
