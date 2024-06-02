import React from 'react';
import ReactDOM from 'react-dom/client';

import Modal from 'react-modal';

import App from './App';

import './index.css';

const root = document.getElementById('root');

Modal.setAppElement(root);

ReactDOM.createRoot(root).render(<App />);
