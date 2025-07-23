import React, { useEffect, useState } from 'react';
import orderService from '../api/OrderService';
import type { Order } from '../models/OrderDtos/Order';
import type { OrderUpdateDto } from '../models/OrderDtos/OrderUpdate';
import type { CreateOrderDto } from '../models/OrderDtos/CreateOrderDto';

const emptyCreateOrder: CreateOrderDto = {
  customerName: '',
  productName: '',
  quantity: 1,
};

const OrderPage: React.FC = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [newOrder, setNewOrder] = useState<CreateOrderDto>(emptyCreateOrder);
  const [editOrderId, setEditOrderId] = useState<number | null>(null);
  const [editFormData, setEditFormData] = useState<OrderUpdateDto>({
    customerName: '',
    productName: '',
    quantity: 1,
  });

  useEffect(() => {
    fetchOrders();
  }, []);

  const fetchOrders = async () => {
    const result = await orderService.getAllOrders();
    if (result.success) {
      setOrders(result.data);
    }
    setLoading(false);
  };

  const handleCreate = async () => {
    const result = await orderService.createOrder(newOrder);
    if (result.success) {
      fetchOrders();
      setNewOrder(emptyCreateOrder);
    }
  };

  const handleDelete = async (id: number) => {
    const result = await orderService.deleteOrder(id);
    if (result.success) {
      fetchOrders();
    }
  };

  const handleUpdate = async () => {
    if (editOrderId !== null && !isNaN(editOrderId)) {
      const result = await orderService.updateOrder(editOrderId, editFormData);
      if (result.success) {
        fetchOrders();
        setEditOrderId(null);
      }
    }
  };

  const handleEditClick = (order: Order) => {
    setEditOrderId(order.orderId);
    setEditFormData({
      customerName: order.customerName,
      productName: order.productName,
      quantity: order.quantity,
    });
  };

  return (
    <div className="container-fluid min-h-screen bg-light py-5 px-3">
      <div className="row justify-content-center">
        <div className="col-lg-10 col-xl-9">
          <div className="bg-white rounded shadow p-4 mb-5">
            <h2 className="text-primary mb-4">Yeni Sipariş Oluştur</h2>
            <div className="row g-3">
              <div className="col-md-6">
                <input
                  type="text"
                  placeholder="Müşteri Adı"
                  value={newOrder.customerName}
                  onChange={(e) =>
                    setNewOrder({ ...newOrder, customerName: e.target.value })
                  }
                  className="form-control"
                />
              </div>
              <div className="col-md-6">
                <input
                  type="text"
                  placeholder="Ürün Adı"
                  value={newOrder.productName}
                  onChange={(e) =>
                    setNewOrder({ ...newOrder, productName: e.target.value })
                  }
                  className="form-control"
                />
              </div>
              <div className="col-md-6">
                <input
                  type="number"
                  placeholder="Miktar"
                  value={newOrder.quantity}
                  onChange={(e) =>
                    setNewOrder({ ...newOrder, quantity: parseInt(e.target.value) })
                  }
                  className="form-control"
                />
              </div>
            </div>
            <div className="mt-3 text-end">
              <button onClick={handleCreate} className="btn btn-primary">
                Ekle
              </button>
            </div>
          </div>

          <div className="bg-white rounded shadow p-4">
            <h3 className="text-dark mb-3">Sipariş Listesi</h3>
            {loading ? (
              <p>Yükleniyor...</p>
            ) : (
              <div className="table-responsive">
                <table className="table table-bordered table-hover">
                  <thead className="table-light">
                    <tr>
                      <th>Müşteri Adı</th>
                      <th>Ürün Adı</th>
                      <th>Miktar</th>
                      <th className="text-center">İşlemler</th>
                    </tr>
                  </thead>
                  <tbody>
                    {orders.map((order) => (
                      <tr key={order.orderId}>
                        <td>{order.customerName}</td>
                        <td>{order.productName}</td>
                        <td>{order.quantity}</td>
                        <td className="text-center">
                          <button
                            className="btn btn-warning btn-sm me-2"
                            onClick={() => handleEditClick(order)}
                          >
                            Güncelle
                          </button>
                          <button
                            className="btn btn-danger btn-sm"
                            onClick={() => handleDelete(order.orderId)}
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
      {editOrderId !== null && (
        <div className="position-fixed top-0 start-0 w-100 h-100 bg-dark bg-opacity-50 d-flex justify-content-center align-items-center zindex-tooltip">
          <div className="bg-white rounded shadow p-4 w-100" style={{ maxWidth: '500px' }}>
            <h4 className="mb-4">Sipariş Güncelle</h4>
            <div className="mb-3">
              <label className="form-label">Müşteri Adı</label>
              <input
                type="text"
                value={editFormData.customerName}
                onChange={(e) =>
                  setEditFormData({ ...editFormData, customerName: e.target.value })
                }
                className="form-control"
              />
            </div>
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
            <div className="text-end">
              <button
                className="btn btn-secondary me-2"
                onClick={() => setEditOrderId(null)}
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

export default OrderPage;
