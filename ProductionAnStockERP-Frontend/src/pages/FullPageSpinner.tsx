import React from 'react';
import '../assets/css/FullPageSpinner.css';

const FullPageSpinner: React.FC = () => {
  return (
    <div className="spinner-overlay">
     <div className="d-flex justify-content-center align-items-center vh-100">
             <div className="text-center">
               <div className="spinner-border text-primary" role="status" />
               <p className="mt-3">Kullanıcı bilgileri yükleniyor...</p>
             </div>
           </div>
    </div>
  );
};

export default FullPageSpinner;